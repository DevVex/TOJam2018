using UnityEngine;

namespace Crosstales.TrueRandom.Module
{
    /// <summary>
    /// This generator will generate true random Vector2 in configurable intervals.
    /// </summary>
    public abstract class ModuleVector2 : BaseModule
    {
        #region Variables

        private static readonly System.Random rnd = new System.Random();

        private static System.Collections.Generic.List<Vector2> result = new System.Collections.Generic.List<Vector2>();

        private static bool isRunning = false;

        #endregion


        #region Events

        public delegate void GenerateStart();
        public delegate void GenerateFinished(System.Collections.Generic.List<Vector2> result, string id);

        /// <summary>Event to get a message when generating Vector2 has started.</summary>
        public static event GenerateStart OnGenerateStart
        {
            add { _onGenerateStart += value; }
            remove { _onGenerateStart -= value; }
        }

        /// <summary>Event to get a message with the generated Vector2 when finished.</summary>
        public static event GenerateFinished OnGenerateFinished
        {
            add { _onGenerateFinished += value; }
            remove { _onGenerateFinished -= value; }
        }

        private static GenerateStart _onGenerateStart;
        private static GenerateFinished _onGenerateFinished;

        #endregion


        #region Static properties

        /// <summary>Returns the list of Vector2 from the last generation.</summary>
        /// <returns>List of Vector2 from the last generation.</returns>
        public static System.Collections.Generic.List<Vector2> Result
        {
            get
            {
                return new System.Collections.Generic.List<Vector2>(result);
            }
        }

        #endregion


        #region Public methods

        /// <summary>Generates random Vector2.</summary>
        /// <param name="min">Smallest possible Vector2 (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="max">Biggest possible Vector2 (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="number">How many vectors you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        /// <param name="prng">Use Pseudo-Random-Number-Generator (default: false, optional)</param>
        /// <param name="silent">Ignore callbacks (default: false, optional)</param>
        /// <param name="id">id to identifiy the generated result (optional)</param>
        public static System.Collections.IEnumerator Generate(Vector2 min, Vector2 max, int number = 1, bool prng = false, bool silent = false, string id = "")
        {
            int _number = Mathf.Clamp(number, 1, 10000);
            Vector2 _min = validateVector(min);
            Vector2 _max = validateVector(max);

            fixMinMax(ref _min, ref _max);

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

                            System.Collections.Generic.List<Vector2> _result = new System.Collections.Generic.List<Vector2>(_number);

                            // x-axis
                            yield return ModuleFloat.Generate(_min.x, _max.x, _number, prng, true);

                            foreach (float x in ModuleFloat.Result)
                            {
                                _result.Add(new Vector2(x, 0));
                            }

                            // y-axis
                            yield return ModuleFloat.Generate(_min.y, _max.y, _number, prng, true);

                            Vector2 value;
                            for (int ii = 0; ii < _result.Count && ii < ModuleFloat.Result.Count; ii++)
                            {
                                value = _result[ii];
                                value.y = ModuleFloat.Result[ii];
                                _result[ii] = value;
                            }

                            result = _result;
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

        /// <summary>Generates random Vector2 with the C#-standard Pseudo-Random-Number-Generator.</summary>
        /// <param name="min">Smallest possible Vector2</param>
        /// <param name="max">Biggest possible Vector2</param>
        /// <param name="number">How many Vector2 you want to generate (default: 1, optional)</param>
        /// <returns>List with the generated Vector2.</returns>
        public static System.Collections.Generic.List<Vector2> GeneratePRNG(Vector2 min, Vector2 max, int number = 1)
        {
            int _number = Mathf.Abs(number);

            Vector2 _min = validateVector(min);
            Vector2 _max = validateVector(max);

            System.Collections.Generic.List<Vector2> _result = new System.Collections.Generic.List<Vector2>(_number);

            fixMinMax(ref _min, ref _max);

            for (int ii = 0; ii < _number; ii++)
            {
                _result.Add(new Vector2((float)(rnd.NextDouble() * (_max.x - _min.x) + _min.x), (float)(rnd.NextDouble() * (_max.y - _min.y) + _min.y)));
            }

            return _result;
        }

        #endregion


        #region Private methods

        private static Vector2 validateVector(Vector2 vec)
        {
            return new Vector2(Mathf.Clamp(vec.x, -1000000000f, 1000000000f), Mathf.Clamp(vec.y, -1000000000f, 1000000000f));
        }

        private static void fixMinMax(ref Vector2 vectorMin, ref Vector2 vectorMax)
        {
            Vector2 _vectorMin = vectorMin;
            Vector2 _vectorMax = vectorMax;

            if (vectorMin.x > vectorMax.x)
            {
                vectorMin.x = _vectorMax.x;
                vectorMax.x = _vectorMin.x;
            }

            if (vectorMin.y > vectorMax.y)
            {
                vectorMin.y = _vectorMax.y;
                vectorMax.y = _vectorMin.y;
            }
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

        private static void onGenerateFinished(System.Collections.Generic.List<Vector2> result, string id)
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

        /// <summary>Generates random Vector2 (Editor only).</summary>
        /// <param name="min">Smallest possible Vector2 (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="max">Biggest possible Vector2 (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="number">How many Vector2 you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        /// <param name="prng">Use Pseudo-Random-Number-Generator (default: false, optional)</param>
        /// <param name="id">id to identifiy the generated result (optional)</param>
        /// <returns>List with the generated Vector2.</returns>
        public static System.Collections.Generic.List<Vector2> GenerateInEditor(Vector2 min, Vector2 max, int number = 1, bool prng = false, string id = "")
        {
            int _number = Mathf.Clamp(number, 1, 10000);
            Vector2 _min = validateVector(min);
            Vector2 _max = validateVector(max);

            fixMinMax(ref _min, ref _max);

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

                        System.Collections.Generic.List<Vector2> _result = new System.Collections.Generic.List<Vector2>(_number);

                        // x-axis
                        ModuleFloat.GenerateInEditor(_min.x, _max.x, _number);

                        foreach (float x in ModuleFloat.Result)
                        {
                            _result.Add(new Vector2(x, 0));
                        }

                        // y-axis
                        ModuleFloat.GenerateInEditor(_min.y, _max.y, _number);

                        Vector2 value;
                        for (int ii = 0; ii < _result.Count && ii < ModuleFloat.Result.Count; ii++)
                        {
                            value = _result[ii];
                            value.y = ModuleFloat.Result[ii];
                            _result[ii] = value;
                        }

                        result = _result;

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