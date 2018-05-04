using UnityEngine;

namespace Crosstales.TrueRandom.Module
{
    /// <summary>
    /// This module will generate true random integers in configurable intervals.
    /// </summary>
    public abstract class ModuleInteger : BaseModule
    {
        #region Variables

        private static readonly System.Random rnd = new System.Random();

        //[Tooltip("List of the generated integers.")]
        private static System.Collections.Generic.List<int> result = new System.Collections.Generic.List<int>();

        private static bool isRunning = false;

        #endregion


        #region Events

        public delegate void GenerateStart();
        public delegate void GenerateFinished(System.Collections.Generic.List<int> result, string id);

        /// <summary>Event to get a message when generating integers has started.</summary>
        public static event GenerateStart OnGenerateStart
        {
            add { _onGenerateStart += value; }
            remove { _onGenerateStart -= value; }
        }

        /// <summary>Event to get a message with the generated integers when finished.</summary>
        public static event GenerateFinished OnGenerateFinished
        {
            add { _onGenerateFinished += value; }
            remove { _onGenerateFinished -= value; }
        }

        private static GenerateStart _onGenerateStart;
        private static GenerateFinished _onGenerateFinished;

        #endregion


        #region Static properties

        /// <summary>Returns the list of integers from the last generation.</summary>
        /// <returns>List of integers from the last generation.</returns>
        public static System.Collections.Generic.List<int> Result
        {
            get
            {
                return new System.Collections.Generic.List<int>(result);
            }
        }

        #endregion


        #region Public methods

        /// <summary>Generates random integers.</summary>
        /// <param name="min">Smallest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="max">Biggest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="number">How many numbers you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        /// <param name="prng">Use Pseudo-Random-Number-Generator (default: false, optional)</param>
        /// <param name="silent">Ignore callbacks (default: false, optional)</param>
        /// <param name="id">id to identifiy the generated result (optional)</param>
        public static System.Collections.IEnumerator Generate(int min, int max, int number = 1, bool prng = false, bool silent = false, string id = "")
        {
            int _number = Mathf.Clamp(number, 1, 10000);
            int _min = Mathf.Clamp(min, -1000000000, 1000000000);
            int _max = Mathf.Clamp(max, -1000000000, 1000000000);

            if (_min > _max)
            {
                Debug.LogWarning("'min' value is larger than 'max' value - switching values.");

                _min = max;
                _max = min;
            }

            if (!silent)
                onGenerateStart();

            if (_min == _max)
            {
                result = GeneratePRNG(_min, _max, _number);
            }
            else
            {
                if (prng)
                {
                    result = GeneratePRNG(_min, _max, _number);
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
                                string url = Util.Constants.GENERATOR_URL + "integers/?num=" + _number + "&min=" + _min + "&max=" + _max + "&col=1&base=10&format=plain&rnd=new";

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

                                        int value = 0;
                                        foreach (string valueAsString in _result)
                                        {
                                            if (int.TryParse(valueAsString, out value))
                                            {
                                                result.Add(value);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        onErrorInfo(www.error);
                                        Debug.LogWarning("Could not read from url: " + www.error);

                                        result = GeneratePRNG(_min, _max, _number);
                                    }
                                }

                                if (Util.Config.DEBUG)
                                    Debug.Log("Quota after: " + ModuleQuota.Quota);
                            }
                            else
                            {
                                string msg = "Quota exceeded - using standard prng now!";
                                Debug.LogError(msg);
                                onErrorInfo(msg);

                                result = GeneratePRNG(_min, _max, _number);
                            }
                        }
                        else
                        {
                            string msg = "No Internet access available - using standard prng now!";
                            Debug.LogError(msg);
                            onErrorInfo(msg);

                            result = GeneratePRNG(_min, _max, _number);
                        }

                        isRunning = false;
                    }
                    else
                    {
                        Debug.LogWarning("There is already a request running - please try again later!");
                    }

                }
            }

            if (!silent)
                onGenerateFinished(Result, id);
        }

        /// <summary>Generates random integers with the C#-standard Pseudo-Random-Number-Generator.</summary>
        /// <param name="min">Smallest possible number</param>
        /// <param name="max">Biggest possible number</param>
        /// <param name="number">How many numbers you want to generate (default: 1, optional)</param>
        /// <returns>List with the generated integers.</returns>
        public static System.Collections.Generic.List<int> GeneratePRNG(int min, int max, int number = 1)
        {
            int _number = Mathf.Abs(number);
            int _min = min;
            int _max = max;

            System.Collections.Generic.List<int> _result = new System.Collections.Generic.List<int>(_number);

            if (min > max)
            {
                Debug.LogWarning("'min' value is larger than 'max' value - switching values.");

                min = _max;
                max = _min;
            }

            for (int ii = 0; ii < _number; ii++)
            {
                _result.Add(rnd.Next(_min, _max + 1));
            }

            return _result;
        }

        #endregion


        #region Private methods

        private static void onGenerateStart()
        {
            if (Util.Config.DEBUG)
                Debug.Log("onGenerateStart");

            if (_onGenerateStart != null)
            {
                _onGenerateStart();
            }
        }

        private static void onGenerateFinished(System.Collections.Generic.List<int> result, string id)
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

        /// <summary>Generates random integers with the C#-standard Pseudo-Random-Number-Generator (Editor only).</summary>
        /// <param name="min">Smallest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="max">Biggest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="number">How many numbers you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        /// <param name="prng">Use Pseudo-Random-Number-Generator (default: false, optional)</param>
        /// <param name="id">id to identifiy the generated result (optional)</param>
        /// <returns>List with the generated integers.</returns>
        public static System.Collections.Generic.List<int> GenerateInEditor(int min, int max, int number = 1, bool prng = false, string id = "")
        {
            int _number = Mathf.Clamp(number, 1, 10000);
            int _min = Mathf.Clamp(min, -1000000000, 1000000000);
            int _max = Mathf.Clamp(max, -1000000000, 1000000000);

            onGenerateStart();

            if (_min > _max)
            {
                Debug.LogWarning("'min' value is larger than 'max' value - switching values.");

                _min = max;
                _max = min;
            }

            if (_min == _max)
            {
                result = GeneratePRNG(_min, _max, _number);
            }
            else
            {

#if UNITY_WSA
                result = GeneratePRNG(_min, _max, _number);
#else
                if (prng)
                {
                    result = GeneratePRNG(_min, _max, _number);
                }
                else
                {
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

                                    string url = Util.Constants.GENERATOR_URL + "integers/?num=" + _number + "&min=" + _min + "&max=" + _max + "&col=1&base=10&format=plain&rnd=new";

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

                                            int value = 0;
                                            foreach (string valueAsString in _result)
                                            {
                                                if (int.TryParse(valueAsString, out value))
                                                {
                                                    result.Add(value);
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

                            result = GeneratePRNG(_min, _max, _number);
                        }

                        isRunning = false;
                    }
                    else
                    {
                        Debug.LogWarning("There is already a request running - please try again later!");
                    }

                }
#endif
            }

            onGenerateFinished(Result, id);

            return Result;
        }

#endif

        #endregion
    }
}
// © 2016-2018 crosstales LLC (https://www.crosstales.com)
