using UnityEngine;

namespace Crosstales.TrueRandom.Module
{
    /// <summary>
    /// This module will generate true random floats in configurable intervals.
    /// </summary>
    public abstract class ModuleFloat : BaseModule
    {
        #region Variables

        private static readonly System.Random rnd = new System.Random();

        private static System.Collections.Generic.List<float> result = new System.Collections.Generic.List<float>();

        private static bool isRunning = false;
        #endregion


        #region Events

        public delegate void GenerateStart();
        public delegate void GenerateFinished(System.Collections.Generic.List<float> result, string id);

        /// <summary>Event to get a message when generating floats has started.</summary>
        public static event GenerateStart OnGenerateStart
        {
            add { _onGenerateStart += value; }
            remove { _onGenerateStart -= value; }
        }


        /// <summary>Event to get a message with the generated floats when finished.</summary>
        public static event GenerateFinished OnGenerateFinished
        {
            add { _onGenerateFinished += value; }
            remove { _onGenerateFinished -= value; }
        }

        private static GenerateStart _onGenerateStart;
        private static GenerateFinished _onGenerateFinished;

        #endregion


        #region Static properties

        /// <summary>Returns the list of floats from the last generation.</summary>
        /// <returns>List of floats from the last generation.</returns>
        public static System.Collections.Generic.List<float> Result
        {
            get
            {
                return new System.Collections.Generic.List<float>(result);
            }
        }

        #endregion


        #region Public methods

        /// <summary>Generates random floats.</summary>
        /// <param name="min">Smallest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="max">Biggest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="number">How many numbers you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        /// <param name="prng">Use Pseudo-Random-Number-Generator (default: false, optional)</param>
        /// <param name="silent">Ignore callbacks (default: false, optional)</param>
        /// <param name="id">id to identifiy the generated result (optional)</param>
        public static System.Collections.IEnumerator Generate(float min, float max, int number = 1, bool prng = false, bool silent = false, string id = "")
        {
            int _number = Mathf.Clamp(number, 1, 10000);
            float _min = Mathf.Clamp(min, -1000000000f, 1000000000f);
            float _max = Mathf.Clamp(max, -1000000000f, 1000000000f);

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
                        if (Util.Helper.isInternetAvailable)
                        {
                            isRunning = true;

                            double factorMax = _max != 0 ? (double)(1000000000f / (Mathf.Abs(_max))) : 1f;
                            double factorMin = _min != 0 ? (double)(1000000000f / (Mathf.Abs(_min))) : 1f;

                            double factor = 0;

                            if (factorMax > factorMin && factorMin != 1f)
                            {
                                factor = factorMin;
                            }
                            else if (factorMin > factorMax && factorMax != 1f)
                            {
                                factor = factorMax;
                            }
                            else
                            {
                                if (_min != 0)
                                {
                                    factor = factorMin;
                                }
                                else
                                {
                                    factor = factorMax;
                                }
                            }

                            yield return ModuleInteger.Generate((int)(_min * factor), (int)(_max * factor), _number, prng, true);

                            result.Clear();
                            foreach (int value in ModuleInteger.Result)
                            {
                                result.Add((float)value / (float)factor);

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

        /// <summary>Generates random floats with the C#-standard Pseudo-Random-Number-Generator.</summary>
        /// <param name="min">Smallest possible number</param>
        /// <param name="max">Biggest possible number</param>
        /// <param name="number">How many numbers you want to generate (default: 1, optional)</param>
        /// <returns>List with the generated floats.</returns>
        public static System.Collections.Generic.List<float> GeneratePRNG(float min, float max, int number = 1)
        {
            int _number = Mathf.Abs(number);
            float _min = min;
            float _max = max;
            System.Collections.Generic.List<float> _result = new System.Collections.Generic.List<float>(_number);

            if (_min > _max)
            {
                Debug.LogWarning("'min' value is larger than 'max' value - switching values.");

                _min = max;
                _max = min;
            }

            for (int ii = 0; ii < _number; ii++)
            {
                _result.Add((float)(rnd.NextDouble() * (_max - _min) + _min));
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

        private static void onGenerateFinished(System.Collections.Generic.List<float> result, string id)
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

        /// <summary>Generates random floats (Editor only).</summary>
        /// <param name="min">Smallest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="max">Biggest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="number">How many numbers you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        /// <param name="prng">Use Pseudo-Random-Number-Generator (default: false, optional)</param>
        /// <param name="id">id to identifiy the generated result (optional)</param>
        /// <returns>List with the generated floats.</returns>
        public static System.Collections.Generic.List<float> GenerateInEditor(float min, float max, int number = 1, bool prng = false, string id = "")
        {
            int _number = Mathf.Clamp(number, 1, 10000);
            float _min = Mathf.Clamp(min, -1000000000f, 1000000000f);
            float _max = Mathf.Clamp(max, -1000000000f, 1000000000f);

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
                if (prng)
                {
                    result = GeneratePRNG(_min, _max, _number);
                }
                else
                {
                    if (!isRunning)
                    {
                        isRunning = true;

                        double factorMax = _max != 0 ? (double)(1000000000f / (Mathf.Abs(_max))) : 1f;
                        double factorMin = _min != 0 ? (double)(1000000000f / (Mathf.Abs(_min))) : 1f;

                        double factor = 0;

                        if (factorMax > factorMin && factorMin != 1f)
                        {
                            factor = factorMin;
                        }
                        else if (factorMin > factorMax && factorMax != 1f)
                        {
                            factor = factorMax;
                        }
                        else
                        {
                            if (_min != 0)
                            {
                                factor = factorMin;
                            }
                            else
                            {
                                factor = factorMax;
                            }
                        }

                        result.Clear();
                        foreach (int value in ModuleInteger.GenerateInEditor((int)(_min * factor), (int)(_max * factor), _number, prng))
                        {
                            result.Add((float)value / (float)factor);

                        }

                        isRunning = false;
                    }
                    else
                    {
                        Debug.LogWarning("There is already a request running - please try again later!");
                    }
                }
            }

            onGenerateFinished(Result, id);

            return Result;
        }

#endif

        #endregion
    }
}
// © 2017-2018 crosstales LLC (https://www.crosstales.com)
