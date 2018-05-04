using UnityEngine;

namespace Crosstales.TrueRandom.Module
{
    /// <summary>
    /// This module will generate true random strings of various length and character compositions.
    /// </summary>
    public abstract class ModuleString : BaseModule
    {
        #region Variables

        private static readonly System.Random rnd = new System.Random();

        //[Tooltip("List of the randomized strings.")]
        private static System.Collections.Generic.List<string> result = new System.Collections.Generic.List<string>();

        private static bool isRunning = false;

        #endregion


        #region Events

        public delegate void GenerateStart();
        public delegate void GenerateFinished(System.Collections.Generic.List<string> result, string id);

        /// <summary>Event to get a message when generating strings has started.</summary>
        public static event GenerateStart OnGenerateStart
        {
            add { _onGenerateStart += value; }
            remove { _onGenerateStart -= value; }
        }

        /// <summary>Event to get a message with the generated strings when finished.</summary>
        public static event GenerateFinished OnGenerateFinished
        {
            add { _onGenerateFinished += value; }
            remove { _onGenerateFinished -= value; }
        }

        private static GenerateStart _onGenerateStart;
        private static GenerateFinished _onGenerateFinished;

        #endregion


        #region Static properties

        /// <summary>Returns the list of strings from the last generation.</summary>
        /// <returns>List of strings from the last generation.</returns>
        public static System.Collections.Generic.List<string> Result
        {
            get
            {
                return result.GetRange(0, result.Count);
            }
        }

        #endregion


        #region Public methods

        /// <summary>Generates random strings.</summary>
        /// <param name="length">How long the strings should be (range: 1 - 20)</param>
        /// <param name="number">How many strings you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        /// <param name="digits">Allow digits (0-9) (default: true, optional)</param>
        /// <param name="upper">Allow uppercase letters (default: true, optional)</param>
        /// <param name="lower">Allow lowercase letters (default: true, optional)</param>
        /// <param name="unique">String shoud be unique (default: false, optional)</param>
        /// <param name="prng">Use Pseudo-Random-Number-Generator (default: false, optional)</param>
        /// <param name="silent">Ignore callbacks (default: false, optional)</param>
        /// <param name="id">id to identifiy the generated result (optional)</param>
        public static System.Collections.IEnumerator Generate(int length, int number = 1, bool digits = true, bool upper = true, bool lower = true, bool unique = false, bool prng = false, bool silent = false, string id = "")
        {
            int _length = Mathf.Clamp(length, 1, 20);
            int _number = calcMaxNumber(number, _length, digits, upper, lower, unique);

            if (!digits && !upper && !lower)
            {
                onErrorInfo("'digits', 'upper' and 'lower' are 'false' - string generation not possible!");
            }
            else
            {

                if (!silent)
                    onGenerateStart();

                if (prng)
                {
                    result = GeneratePRNG(_length, _number, digits, upper, lower, unique);
                }
                else
                {
                    if (!isRunning)
                    {
                        isRunning = true;

                        if (Util.Helper.isInternetAvailable)
                        {
                            if (Util.Config.DEBUG)
                                Debug.Log("Quota before: " + ModuleQuota.Quota);

                            if (ModuleQuota.Quota > 0)
                            {
                                string url = Util.Constants.GENERATOR_URL + "strings/?num=" + _number + "&len=" + _length + "&digits=" + boolToString(digits) + "&upperalpha=" + boolToString(upper) + "&loweralpha=" + boolToString(lower) + "&unique=" + boolToString(unique) + "&format=plain&rnd=new";

                                using (WWW www = new WWW(url))
                                {

                                    do
                                    {
                                        yield return www;
                                    } while (!www.isDone);

                                    if (string.IsNullOrEmpty(www.error))
                                    {
                                        result.Clear();
                                        string[] _result = System.Text.RegularExpressions.Regex.Split(www.text, "\r\n?|\n", System.Text.RegularExpressions.RegexOptions.Singleline);

                                        foreach (string valueAsString in _result)
                                        {
                                            if (!string.IsNullOrEmpty(valueAsString))
                                            {
                                                result.Add(valueAsString);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        onErrorInfo(www.error);
                                        Debug.LogWarning("Could not read from url: " + www.error);

                                        result = GeneratePRNG(_length, _number, digits, upper, lower, unique);
                                    }
                                }

                                if (Util.Config.DEBUG)
                                {
                                    Debug.Log("Quota after: " + ModuleQuota.Quota);
                                }
                            }
                            else
                            {
                                string msg = "Quota exceeded - using standard prng now!";
                                Debug.LogError(msg);
                                onErrorInfo(msg);

                                result = GeneratePRNG(_length, _number, digits, upper, lower, unique);
                            }
                        }
                        else
                        {
                            string msg = "No Internet access available - using standard prng now!";
                            Debug.LogError(msg);
                            onErrorInfo(msg);

                            result = GeneratePRNG(_length, _number, digits, upper, lower, unique);
                        }

                        isRunning = false;
                    }
                    else
                    {
                        Debug.LogWarning("There is already a request running - please try again later!");
                    }
                }

                if (!silent)
                    onGenerateFinished(Result, id);
            }


        }

        /// <summary>Generates random strings with the C#-standard Pseudo-Random-Number-Generator.</summary>
        /// <param name="length">How long the strings should be</param>
        /// <param name="number">How many strings you want to generate (default: 1, optional)</param>
        /// <param name="digits">Allow digits (0-9) (default: true, optional)</param>
        /// <param name="upper">Allow uppercase (A-Z) letters (default: true, optional)</param>
        /// <param name="lower">Allow lowercase (a-z) letters (default: true, optional)</param>
        /// <param name="unique">String shoud be unique (default: false, optional)</param>
        /// <returns>List with the generated strings.</returns>
        public static System.Collections.Generic.List<string> GeneratePRNG(int length, int number = 1, bool digits = true, bool upper = true, bool lower = true, bool unique = false)
        {
            int _length = Mathf.Abs(length);
            int _number = calcMaxNumber(number, _length, digits, upper, lower, unique);
            System.Collections.Generic.List<string> _result = new System.Collections.Generic.List<string>(_number);

            string glyphs = string.Empty;

            if (upper)
            {
                glyphs += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }

            if (lower)
            {
                glyphs += "abcdefghijklmnopqrstuvwxyz";
            }

            if (digits)
            {
                glyphs += "1234567890";
            }

            string s = string.Empty;

            for (int i = 0; i < _number; i++)
            {
                if (unique)
                {
                    bool isNotUnique = false;
                    do
                    {
                        isNotUnique = false;
                        s = string.Empty;
                        for (int ii = 0; ii < _length; ii++)
                        {
                            s += glyphs[rnd.Next(0, glyphs.Length)];
                        }
                        foreach (string str in result)
                        {
                            if (str == s)
                            {
                                isNotUnique = true;
                            }
                        }
                    } while (isNotUnique);
                }
                else
                {
                    s = string.Empty;
                    for (int ii = 0; ii < _length; ii++)
                    {
                        s += glyphs[rnd.Next(0, glyphs.Length)];
                    }
                }
                _result.Add(s);
            }

            return _result;
        }

        #endregion


        #region Private methods

        private static int calcMaxNumber(int number, int length, bool digits, bool upper, bool lower, bool unique)
        {
            int _number = Mathf.Clamp(number, 1, 10000);

            if (unique && length > 0 && length <= 10)
            {
                double basis = 0d;

                if (digits)
                {
                    basis += 10d;
                }

                if (upper)
                {
                    basis += 26d;
                }

                if (lower)
                {
                    basis += 26d;
                }

                if (basis > 0d)
                {
                    long maxNumber = (long)(System.Math.Pow(basis, length));

                    if (maxNumber < number)
                    {
                        Debug.LogWarning("Too many numbers requested with 'unique' on - result reduced to " + maxNumber + " numbers!");
                        _number = (int)maxNumber;
                    }
                }
            }

            return _number;
        }

        private static string boolToString(bool value)
        {
            return value ? "on" : "off";
        }

        private static void onGenerateStart()
        {
            if (Util.Config.DEBUG)
                Debug.Log("onGenerateStart");

            if (_onGenerateStart != null)
            {
                _onGenerateStart();
            }
        }

        private static void onGenerateFinished(System.Collections.Generic.List<string> result, string id)
        {
            if (Util.Config.DEBUG)
                Debug.Log("onGenerateFinished: " + result.Count);

            if (_onGenerateFinished != null)
            {
                _onGenerateFinished(result, id);
            }
        }

        #endregion


        #region Editor-only methods

#if UNITY_EDITOR

        /// <summary>Generates random strings (Editor only).</summary>
        /// <param name="length">How long the strings should be (range: 1 - 20)</param>
        /// <param name="number">How many strings you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        /// <param name="digits">Allow digits (0-9) (default: true, optional)</param>
        /// <param name="upper">Allow uppercase letters (default: true, optional)</param>
        /// <param name="lower">Allow lowercase letters (default: true, optional)</param>
        /// <param name="unique">String shoud be unique (default: false, optional)</param>
        /// <param name="prng">Use Pseudo-Random-Number-Generator (default: false, optional)</param>
        /// <param name="id">id to identifiy the generated result (optional)</param>
        /// <returns>List with the generated strings.</returns>
        public static System.Collections.Generic.List<string> GenerateInEditor(int length, int number = 1, bool digits = true, bool upper = true, bool lower = true, bool unique = false, bool prng = false, string id = "")
        {
            int _length = Mathf.Clamp(length, 1, 20);
            int _number = calcMaxNumber(number, _length, digits, upper, lower, unique);

            onGenerateStart();

            if (!digits && !upper && !lower)
            {
                Debug.LogError("'digits', 'upper' and 'lower' are 'false' - string generation not possible!");
            }
            else
            {
                if (prng)
                {
                    result = GeneratePRNG(_length, _number, digits, upper, lower, unique);
                }
                else
                {

#if UNITY_WSA
                    result = GeneratePRNG(_length, _number, digits, upper, lower, unique);
#else
                    if (!isRunning)
                    {
                        isRunning = true;

                        if (Util.Config.DEBUG)
                            Debug.Log("Quota before: " + ModuleQuota.Quota);

                        if (ModuleQuota.Quota > 0)
                        {
                            try
                            {
                                System.Net.ServicePointManager.ServerCertificateValidationCallback = Util.Helper.RemoteCertificateValidationCallback;

                                using (System.Net.WebClient client = new Common.Util.CTWebClient())
                                {

                                    string url = Util.Constants.GENERATOR_URL + "strings/?num=" + _number + "&len=" + _length + "&digits=" + boolToString(digits) + "&upperalpha=" + boolToString(upper) + "&loweralpha=" + boolToString(lower) + "&unique=" + boolToString(unique) + "&format=plain&rnd=new";

                                    if (Util.Config.DEBUG)
                                        Debug.Log("URL: " + url);

                                    using (System.IO.Stream stream = client.OpenRead(url))
                                    {
                                        using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                                        {
                                            string content = reader.ReadToEnd();

                                            if (Util.Config.DEBUG)
                                                Debug.Log(content);

                                            result.Clear();
                                            string[] _result = System.Text.RegularExpressions.Regex.Split(content, "\r\n?|\n", System.Text.RegularExpressions.RegexOptions.Singleline);

                                            foreach (string valueAsString in _result)
                                            {
                                                if (!string.IsNullOrEmpty(valueAsString))
                                                {
                                                    result.Add(valueAsString);
                                                }
                                            }

                                            if (Util.Config.SHOW_QUOTA)
                                            {
                                                ModuleQuota.GetQuotaInEditor();
                                            }
                                        }
                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {
                                Debug.LogError(ex);
                            }
                        }
                        else
                        {
                            Debug.LogError("Quota exceeded - using prng now!");

                            result = GeneratePRNG(_length, _number, digits, upper, lower, unique);
                        }

                        isRunning = false;
                    }
                    else
                    {
                        Debug.LogWarning("There is already a request running - please try again later!");
                    }

#endif
                }
            }

            onGenerateFinished(Result, id);

            return Result;
        }

#endif

        #endregion
    }
}
// © 2016-2018 crosstales LLC (https://www.crosstales.com)