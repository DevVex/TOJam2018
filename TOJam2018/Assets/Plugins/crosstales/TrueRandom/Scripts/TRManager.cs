using UnityEngine;

namespace Crosstales.TrueRandom
{
    /// <summary>
    /// The TRManager is the manager for all modules.
    /// </summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [HelpURL("https://www.crosstales.com/media/data/assets/truerandom/api/class_crosstales_1_1_true_random_1_1_t_r_manager.html")]
    public class TRManager : MonoBehaviour
    {
        #region Variables

        [Header("Behaviour Settings")]
        /// <summary>Enable or disable the C#-standard Pseudo-Random-Number-Generator-mode (default: false).</summary>
        [Tooltip("Enable or disable the C#-standard Pseudo-Random-Number-Generator-mode (default: false).")]
        public bool PRNG = false;

        /// <summary>Don't destroy gameobject during scene switches (default: true).</summary>
        [Tooltip("Don't destroy gameobject during scene switches (default: true).")]
        public bool DontDestroy = true;

        private static GameObject go;
        private static TRManager instance;
        private static bool initalized = false;
        private static bool loggedOnlyOneInstance = false;
        private static bool loggedTRIsNull = false;

        private static int generateCount = 0;

        #endregion


        #region Events

        public delegate void GenerateIntegerStart();
        public delegate void GenerateIntegerFinished(System.Collections.Generic.List<int> result, string id);

        public delegate void GenerateFloatStart();
        public delegate void GenerateFloatFinished(System.Collections.Generic.List<float> result, string id);

        public delegate void GenerateSequenceStart();
        public delegate void GenerateSequenceFinished(System.Collections.Generic.List<int> result, string id);

        public delegate void GenerateStringStart();
        public delegate void GenerateStringFinished(System.Collections.Generic.List<string> result, string id);

        public delegate void GenerateVector2Start();
        public delegate void GenerateVector2Finished(System.Collections.Generic.List<Vector2> result, string id);

        public delegate void GenerateVector3Start();
        public delegate void GenerateVector3Finished(System.Collections.Generic.List<Vector3> result, string id);

        public delegate void GenerateVector4Start();
        public delegate void GenerateVector4Finished(System.Collections.Generic.List<Vector4> result, string id);

        public delegate void ErrorInfo(string error);

        public delegate void UpdateQuota(int quota);

        /// <summary>Event to get a message when generating integers has started.</summary>
        public static event GenerateIntegerStart OnGenerateIntegerStart
        {
            add { _onGenerateIntegerStart += value; }
            remove { _onGenerateIntegerStart -= value; }
        }

        /// <summary>Event to get a message with the generated integers when finished.</summary>
        public static event GenerateIntegerFinished OnGenerateIntegerFinished
        {
            add { _onGenerateIntegerFinished += value; }
            remove { _onGenerateIntegerFinished -= value; }
        }

        /// <summary>Event to get a message when generating floats has started.</summary>
        public static event GenerateFloatStart OnGenerateFloatStart
        {
            add { _onGenerateFloatStart += value; }
            remove { _onGenerateFloatStart -= value; }
        }

        /// <summary>Event to get a message with the generated floats when finished.</summary>
        public static event GenerateFloatFinished OnGenerateFloatFinished
        {
            add { _onGenerateFloatFinished += value; }
            remove { _onGenerateFloatFinished -= value; }
        }

        /// <summary>Event to get a message when generating sequence has started.</summary>
        public static event GenerateSequenceStart OnGenerateSequenceStart
        {
            add { _onGenerateSequenceStart += value; }
            remove { _onGenerateSequenceStart -= value; }
        }

        /// <summary>Event to get a message with the generated sequence when finished.</summary>
        public static event GenerateSequenceFinished OnGenerateSequenceFinished
        {
            add { _onGenerateSequenceFinished += value; }
            remove { _onGenerateSequenceFinished -= value; }
        }

        /// <summary>Event to get a message when generating strings has started.</summary>
        public static event GenerateStringStart OnGenerateStringStart
        {
            add { _onGenerateStringStart += value; }
            remove { _onGenerateStringStart -= value; }
        }

        /// <summary>Event to get a message with the generated strings when finished.</summary>
        public static event GenerateStringFinished OnGenerateStringFinished
        {
            add { _onGenerateStringFinished += value; }
            remove { _onGenerateStringFinished -= value; }
        }

        /// <summary>Event to get a message when generating Vector2 has started.</summary>
        public static event GenerateVector2Start OnGenerateVector2Start
        {
            add { _onGenerateVector2Start += value; }
            remove { _onGenerateVector2Start -= value; }
        }

        /// <summary>Event to get a message with the generated Vector2 when finished.</summary>
        public static event GenerateVector2Finished OnGenerateVector2Finished
        {
            add { _onGenerateVector2Finished += value; }
            remove { _onGenerateVector2Finished -= value; }
        }

        /// <summary>Event to get a message when generating Vector3 has started.</summary>
        public static event GenerateVector3Start OnGenerateVector3Start
        {
            add { _onGenerateVector3Start += value; }
            remove { _onGenerateVector3Start -= value; }
        }
        /// <summary>Event to get a message with the generated Vector3 when finished.</summary>
        public static event GenerateVector3Finished OnGenerateVector3Finished
        {
            add { _onGenerateVector3Finished += value; }
            remove { _onGenerateVector3Finished -= value; }
        }
        /// <summary>Event to get a message when generating Vector4 has started.</summary>
        public static event GenerateVector4Start OnGenerateVector4Start
        {
            add { _onGenerateVector4Start += value; }
            remove { _onGenerateVector4Start -= value; }
        }

        /// <summary>Event to get a message with the generated Vector4 when finished.</summary>
        public static event GenerateVector4Finished OnGenerateVector4Finished
        {
            add { _onGenerateVector4Finished += value; }
            remove { _onGenerateVector4Finished -= value; }
        }

        /// <summary>Event to get a message when an error occured.</summary>
        public static event ErrorInfo OnErrorInfo
        {
            add { _onErrorInfo += value; }
            remove { _onErrorInfo -= value; }
        }

        /// <summary>Event to get a message with the current quota.</summary>
        public static event UpdateQuota OnUpdateQuota
        {
            add { _onUpdateQuota += value; }
            remove { _onUpdateQuota -= value; }
        }

        private static GenerateIntegerStart _onGenerateIntegerStart;
        private static GenerateIntegerFinished _onGenerateIntegerFinished;

        private static GenerateFloatStart _onGenerateFloatStart;
        private static GenerateFloatFinished _onGenerateFloatFinished;

        private static GenerateSequenceStart _onGenerateSequenceStart;
        private static GenerateSequenceFinished _onGenerateSequenceFinished;

        private static GenerateStringStart _onGenerateStringStart;
        private static GenerateStringFinished _onGenerateStringFinished;

        private static GenerateVector2Start _onGenerateVector2Start;
        private static GenerateVector2Finished _onGenerateVector2Finished;

        private static GenerateVector3Start _onGenerateVector3Start;
        private static GenerateVector3Finished _onGenerateVector3Finished;

        private static GenerateVector4Start _onGenerateVector4Start;
        private static GenerateVector4Finished _onGenerateVector4Finished;

        private static ErrorInfo _onErrorInfo;

        private static UpdateQuota _onUpdateQuota;

        #endregion


        #region Static properties
        
        private static int generateCounter
        {
            get
            {
                return generateCount;
            }

            set
            {
                if (value < 0)
                {
                    generateCount = 0;
                }
                else
                {
                    generateCount = value;
                }
            }
        }
        
        /// <summary>Enable or disable the C#-standard Pseudo-Random-Number-Generator-mode.</summary>
        public static bool isPRNG
        {
            get
            {
                if (instance == null)
                {
                    return false;
                }
                else
                {
                    return instance.PRNG;
                }
            }

            set
            {
                if (instance != null)
                {
                    instance.PRNG = value;
                }
            }
        }

        /// <summary>Returns the remaining quota in bits from the last check.</summary>
        /// <returns>Remaining quota in bits from the last check.</returns>
        public static int CurrentQuota
        {
            get
            {
                return Module.ModuleQuota.Quota;
            }
        }

        /// <summary>Returns the list of integers from the last generation.</summary>
        /// <returns>List of integers from the last generation.</returns>
        public static System.Collections.Generic.List<int> CurrentIntegers
        {
            get
            {
                return Module.ModuleInteger.Result;
            }
        }

        /// <summary>Returns the list of floats from the last generation.</summary>
        /// <returns>List of floats from the last generation.</returns>
        public static System.Collections.Generic.List<float> CurrentFloats
        {
            get
            {
                return Module.ModuleFloat.Result;
            }
        }

        /// <summary>Returns the sequence from the last generation.</summary>
        /// <returns>Sequence from the last generation.</returns>
        public static System.Collections.Generic.List<int> CurrentSequence
        {
            get
            {
                return Module.ModuleSequence.Result;
            }
        }

        /// <summary>Returns the list of strings from the last generation.</summary>
        /// <returns>List of strings from the last generation.</returns>
        public static System.Collections.Generic.List<string> CurrentStrings
        {
            get
            {
                return Module.ModuleString.Result;
            }
        }

        /// <summary>Returns the list of Vector2 from the last generation.</summary>
        /// <returns>List of Vector2 from the last generation.</returns>
        public static System.Collections.Generic.List<Vector2> CurrentVector2
        {
            get
            {
                return Module.ModuleVector2.Result;
            }
        }

        /// <summary>Returns the list of Vector3 from the last generation.</summary>
        /// <returns>List of Vector3 from the last generation.</returns>
        public static System.Collections.Generic.List<Vector3> CurrentVector3
        {
            get
            {
                return Module.ModuleVector3.Result;
            }
        }

        /// <summary>Returns the list of Vector4 from the last generation.</summary>
        /// <returns>List of Vector4 from the last generation.</returns>
        public static System.Collections.Generic.List<Vector4> CurrentVector4
        {
            get
            {
                return Module.ModuleVector4.Result;
            }
        }

        /// <summary>Checks if True Random is generating numbers on this system.</summary>
        /// <returns>True if True Random is generating numbers on this system.</returns>
        public static bool isGenerating
        {
            get
            {
                return generateCounter > 0;
            }
        }

        #endregion


        #region MonoBehaviour methods

        public void OnEnable()
        {
            if (Util.Helper.isEditorMode || !initalized)
            {
                go = gameObject;

                go.name = Util.Constants.TRUERANDOM_SCENE_OBJECT_NAME;

                instance = this;

                // Subscribe event listeners
                Module.BaseModule.OnErrorInfo += onErrorInfo;
                Module.ModuleQuota.OnUpdateQuota += onUpdateQuota;
                Module.ModuleInteger.OnGenerateStart += onGenerateIntegerStart;
                Module.ModuleInteger.OnGenerateFinished += onGenerateIntegerFinished;
                Module.ModuleFloat.OnGenerateStart += onGenerateFloatStart;
                Module.ModuleFloat.OnGenerateFinished += onGenerateFloatFinished;
                Module.ModuleSequence.OnGenerateStart += onGenerateSequenceStart;
                Module.ModuleSequence.OnGenerateFinished += onGenerateSequenceFinished;
                Module.ModuleString.OnGenerateStart += onGenerateStringStart;
                Module.ModuleString.OnGenerateFinished += onGenerateStringFinished;
                Module.ModuleVector2.OnGenerateStart += onGenerateVector2Start;
                Module.ModuleVector2.OnGenerateFinished += onGenerateVector2Finished;
                Module.ModuleVector3.OnGenerateStart += onGenerateVector3Start;
                Module.ModuleVector3.OnGenerateFinished += onGenerateVector3Finished;
                Module.ModuleVector4.OnGenerateStart += onGenerateVector4Start;
                Module.ModuleVector4.OnGenerateFinished += onGenerateVector4Finished;

                if (!Util.Helper.isEditorMode && DontDestroy)
                {
                    DontDestroyOnLoad(transform.root.gameObject);

                    initalized = true;
                }

                GetQuota();

                if (Util.Config.DEBUG)
                    Debug.Log("Using new instance!");
            }
            else
            {
                if (!Util.Helper.isEditorMode && DontDestroy && instance != this)
                {
                    if (!loggedOnlyOneInstance)
                    {
                        Debug.LogWarning("Only one active instance of '" + Util.Constants.TRUERANDOM_SCENE_OBJECT_NAME + "' allowed in all scenes!" + System.Environment.NewLine + "This object will now be destroyed.");

                        loggedOnlyOneInstance = true;
                    }

                    Destroy(gameObject, 0.2f);
                }

                if (Util.Config.DEBUG)
                    Debug.Log("Using old instance!");
            }
        }

        public void Update()
        {
            if (Util.Helper.isEditorMode)
            {
                if (go != null)
                {
                    go.name = Util.Constants.TRUERANDOM_SCENE_OBJECT_NAME; //ensure name
                }
            }
        }

        public void OnDestroy()
        {
            if (instance == this)
            {
                // Unsubscribe event listeners
                Module.BaseModule.OnErrorInfo -= onErrorInfo;
                Module.ModuleQuota.OnUpdateQuota -= onUpdateQuota;
                Module.ModuleInteger.OnGenerateStart -= onGenerateIntegerStart;
                Module.ModuleInteger.OnGenerateFinished -= onGenerateIntegerFinished;
                Module.ModuleFloat.OnGenerateStart -= onGenerateFloatStart;
                Module.ModuleFloat.OnGenerateFinished -= onGenerateFloatFinished;
                Module.ModuleSequence.OnGenerateStart -= onGenerateSequenceStart;
                Module.ModuleSequence.OnGenerateFinished -= onGenerateSequenceFinished;
                Module.ModuleString.OnGenerateStart -= onGenerateStringStart;
                Module.ModuleString.OnGenerateFinished -= onGenerateStringFinished;
                Module.ModuleVector2.OnGenerateStart -= onGenerateVector2Start;
                Module.ModuleVector2.OnGenerateFinished -= onGenerateVector2Finished;
                Module.ModuleVector3.OnGenerateStart -= onGenerateVector3Start;
                Module.ModuleVector3.OnGenerateFinished -= onGenerateVector3Finished;
                Module.ModuleVector4.OnGenerateStart -= onGenerateVector4Start;
                Module.ModuleVector4.OnGenerateFinished -= onGenerateVector4Finished;
            }
        }

        public void OnApplicationQuit()
        {
            if (instance != null)
            {
                instance.StopAllCoroutines();
            }
        }

        #endregion


        #region Static methods

        /// <summary>
        /// Calculates needed bits (from the quota) for generating random floats.
        /// </summary>
        /// <param name="number">How many numbers (default: 1, optional)</param>
        /// <returns>Needed bits for genarting the floats.</returns>
        public static int CalculateFloat(int number = 1)
        {
            int bitsCounter = 32;

            bitsCounter *= Mathf.Abs(number);

            if (Util.Config.DEBUG)
            {
                Debug.Log(Mathf.Abs(number) + "x this number costs " + bitsCounter + " bits");

                int left = CurrentQuota - bitsCounter;
                int times = bitsCounter == 0 ? int.MaxValue : left / bitsCounter;

                Debug.Log("Quota left after generating float numbers: " + left);
                Debug.Log("You can generate these numbers " + times + "x in the next 24 hours");
            }

            return bitsCounter;
        }

        /// <summary>
        /// Calculates needed bits (from the quota) for generating random integers.
        /// </summary>
        /// <param name="max">Biggest allowed number</param>
        /// <param name="number">How many numbers (default: 1, optional)</param>
        /// <returns>Needed bits for genarting the integers.</returns>
        public static int CalculateInteger(int max, int number = 1)
        {
            int bitsCounter = 0;
            float tmp = Mathf.Abs(max);

            while (tmp >= 1)
            {
                if (tmp % 2 == 0)
                {
                    tmp /= 2;
                }
                else
                {
                    tmp /= 2;
                    tmp -= 0.5f;
                }
                bitsCounter++;
            }

            bitsCounter *= Mathf.Abs(number);

            if (Util.Config.DEBUG)
            {
                Debug.Log(Mathf.Abs(number) + "x this number costs " + bitsCounter + " bits");

                int left = CurrentQuota - bitsCounter;
                int times = bitsCounter == 0 ? int.MaxValue : left / bitsCounter;

                Debug.Log("Quota left after generating integer numbers: " + left);
                Debug.Log("You can generate these numbers " + times + "x in the next 24 hours");
            }

            return bitsCounter;
        }

        /// <summary>
        /// Calculates needed bits (from the quota) for generating a random sequence.
        /// </summary>
        /// <param name="min">Start of the interval</param>
        /// <param name="max">End of the interval</param>
        /// <returns>Needed bits for genarting the sequence.</returns>
        public static int CalculateSequence(int min, int max)
        {
            int _min = min;
            int _max = max;

            if (_min > _max)
            {
                _min = max;
                _max = min;
            }

            if (_min == 0 && _max == 0)
            {
                _max = 1;
            }

            int bitsCounter = 0;

            if (_min != _max)
            {
                bitsCounter = (_max - _min) * 31;
            }

            if (Util.Config.DEBUG)
            {
                Debug.Log(_max - _min + 1 + "x this sequence costs " + bitsCounter + " bits");

                int left = CurrentQuota - bitsCounter;
                int times = bitsCounter == 0 ? int.MaxValue : left / bitsCounter;

                Debug.Log("Quota left after generating sequence: " + left);
                Debug.Log("You can generate these sequence " + times + "x in the next 24 hours");
            }

            return bitsCounter;
        }

        /// <summary>
        /// Calculates needed bits (from the quota) for generating random strings.
        /// </summary>
        /// <param name="length">Length of the strings</param>
        /// <param name="number">How many strings (default: 1, optional)</param>
        /// <returns>Needed bits for genarting the strings.</returns>
        public static int CalculateString(int length, int number = 1)
        {
            int bitsCounter = Mathf.Abs(number) * Mathf.Abs(length) * 30;

            if (Util.Config.DEBUG)
            {
                Debug.Log("It's not possible to calculate exactly how many bits it costs. 1 char costs between 4 and 30 bits");

                Debug.Log("Generating these strings will use between: " + Mathf.Abs(number) * Mathf.Abs(length) * 4 + " and: " + Mathf.Abs(number) * Mathf.Abs(length) * 30 + " bits");

                int left = CurrentQuota - bitsCounter;
                int times = bitsCounter == 0 ? int.MaxValue : left / bitsCounter;

                Debug.Log("Quota left after generating strings: " + left);
                Debug.Log("You can generate these strings " + times + "x in the next 24 hours");
            }

            return bitsCounter;
        }

        /// <summary>
        /// Calculates needed bits (from the quota) for generating random Vector2.
        /// </summary>
        /// <param name="number">How many Vector2 (default: 1, optional)</param>
        /// <returns>Needed bits for genarting the Vector2.</returns>
        public static int CalculateVector2(int number = 1)
        {
            return CalculateFloat(number * 2);
        }

        /// <summary>
        /// Calculates needed bits (from the quota) for generating random Vector3.
        /// </summary>
        /// <param name="number">How many Vector3 (default: 1, optional)</param>
        /// <returns>Needed bits for genarting the Vector3.</returns>
        public static int CalculateVector3(int number = 1)
        {
            return CalculateFloat(number * 3);
        }

        /// <summary>
        /// Calculates needed bits (from the quota) for generating random Vector4.
        /// </summary>
        /// <param name="number">How many Vector4 (default: 1, optional)</param>
        /// <returns>Needed bits for genarting the Vector4.</returns>
        public static int CalculateVector4(int number = 1)
        {
            return CalculateFloat(number * 4);
        }

        /// <summary>Generates random integers.</summary>
        /// <param name="min">Smallest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="max">Biggest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="number">How many numbers you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        /// <param name="id">id to identifiy the generated result (optional)</param>
        public static void GenerateInteger(int min, int max, int number = 1, string id = "")
        {
            if (instance != null)
            {
                instance.StartCoroutine(Module.ModuleInteger.Generate(min, max, number, isPRNG, false, id));
            }
            else
            {
                logTRIsNull();
            }
        }

        /// <summary>Generates random floats.</summary>
        /// <param name="min">Smallest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="max">Biggest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="number">How many numbers you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        /// <param name="id">id to identifiy the generated result (optional)</param>
        public static void GenerateFloat(float min, float max, int number = 1, string id = "")
        {
            if (instance != null)
            {
                instance.StartCoroutine(Module.ModuleFloat.Generate(min, max, number, isPRNG, false, id));
            }
            else
            {
                logTRIsNull();
            }
        }

        /// <summary>Generates random sequence.</summary>
        /// <param name="min">Start of the interval (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="max">End of the interval (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="number">How many numbers you have in the result (max range: max - min, optional)</param>
        /// <param name="id">id to identifiy the generated result (optional)</param>
        public static void GenerateSequence(int min, int max, int number = 0, string id = "")
        {
            if (instance != null)
            {
                instance.StartCoroutine(Module.ModuleSequence.Generate(min, max, number, isPRNG, false, id));
            }
            else
            {
                logTRIsNull();
            }
        }

        /// <summary>Generates random strings.</summary>
        /// <param name="length">How long the strings should be (range: 1 - 20)</param>
        /// <param name="number">How many strings you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        /// <param name="digits">Allow digits (0-9) (default: true, optional)</param>
        /// <param name="upper">Allow uppercase (A-Z) letters (default: true, optional)</param>
        /// <param name="lower">Allow lowercase (a-z) letters (default: true, optional)</param>
        /// <param name="unique">String shoud be unique in the result (default: false, optional)</param>
        /// <param name="id">id to identifiy the generated result (optional)</param>
        public static void GenerateString(int length, int number = 1, bool digits = true, bool upper = true, bool lower = true, bool unique = false, string id = "")
        {
            if (instance != null)
            {
                instance.StartCoroutine(Module.ModuleString.Generate(length, number, digits, upper, lower, unique, isPRNG, false, id));
            }
            else
            {
                logTRIsNull();
            }
        }

        /// <summary>Generates random Vector2.</summary>
        /// <param name="min">Smallest possible Vector2 (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="max">Biggest possible Vector2 (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="number">How many Vector2 you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        /// <param name="id">id to identifiy the generated result (optional)</param>
        public static void GenerateVector2(Vector2 min, Vector2 max, int number = 1, string id = "")
        {
            if (instance != null)
            {
                instance.StartCoroutine(Module.ModuleVector2.Generate(min, max, number, isPRNG, false, id));
            }
            else
            {
                logTRIsNull();
            }
        }

        /// <summary>Generates random Vector3.</summary>
        /// <param name="min">Smallest possible Vector3 (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="max">Biggest possible Vector3 (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="number">How many Vector3 you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        /// <param name="id">id to identifiy the generated result (optional)</param>
        public static void GenerateVector3(Vector3 min, Vector3 max, int number = 1, string id = "")
        {
            if (instance != null)
            {
                instance.StartCoroutine(Module.ModuleVector3.Generate(min, max, number, isPRNG, false, id));
            }
            else
            {
                logTRIsNull();
            }
        }

        /// <summary>Generates random Vector4.</summary>
        /// <param name="min">Smallest possible Vector4 (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="max">Biggest possible Vector4 (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="number">How many Vector4 you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        /// <param name="id">id to identifiy the generated result (optional)</param>
        public static void GenerateVector4(Vector4 min, Vector4 max, int number = 1, string id = "")
        {
            if (instance != null)
            {
                instance.StartCoroutine(Module.ModuleVector4.Generate(min, max, number, isPRNG, false, id));
            }
            else
            {
                logTRIsNull();
            }
        }

        /// <summary>Gets the remaining quota in bits from the server.</summary>
        public static void GetQuota()
        {
            if (instance != null)
            {
                instance.StartCoroutine(Module.ModuleQuota.GetQuota());
            }
            else
            {
                logTRIsNull();
            }
        }

        /// <summary>Generates random integers with the C#-standard Pseudo-Random-Number-Generator.</summary>
        /// <param name="min">Smallest possible number</param>
        /// <param name="max">Biggest possible number</param>
        /// <param name="number">How many numbers you want to generate (default: 1, optional)</param>
        /// <returns>List with the generated integers.</returns>
        public static System.Collections.Generic.List<int> GenerateIntegerPRNG(int min, int max, int number = 1)
        {
            return Module.ModuleInteger.GeneratePRNG(min, max, number);
        }

        /// <summary>Generates random floats with the C#-standard Pseudo-Random-Number-Generator.</summary>
        /// <param name="min">Smallest possible number</param>
        /// <param name="max">Biggest possible number</param>
        /// <param name="number">How many numbers you want to generate (default: 1, optional)</param>
        /// <returns>List with the generated floats.</returns>
        public static System.Collections.Generic.List<float> GenerateFloatPRNG(float min, float max, int number = 1)
        {
            return Module.ModuleFloat.GeneratePRNG(min, max, number);
        }

        /// <summary>Generates a random sequence with the C#-standard Pseudo-Random-Number-Generator.</summary>
        /// <param name="min">Start of the interval</param>
        /// <param name="max">End of the interval</param>
        /// <param name="number">How many numbers you have in the result (max range: max - min, optional)</param>
        /// <returns>List with the generated sequence.</returns>
        public static System.Collections.Generic.List<int> GenerateSequencePRNG(int min, int max, int number = 0)
        {
            return Module.ModuleSequence.GeneratePRNG(min, max, number);
        }

        /// <summary>Generates random strings with the C#-standard Pseudo-Random-Number-Generator.</summary>
        /// <param name="length">How long the strings should be</param>
        /// <param name="number">How many strings you want to generate (default: 1, optional)</param>
        /// <param name="digits">Allow digits (0-9) (default: true, optional)</param>
        /// <param name="upper">Allow uppercase (A-Z) letters (default: true, optional)</param>
        /// <param name="lower">Allow lowercase (a-z) letters (default: true, optional)</param>
        /// <param name="unique">String shoud be unique (default: false, optional)</param>
        /// <returns>List with the generated strings.</returns>
        public static System.Collections.Generic.List<string> GenerateStringPRNG(int length, int number = 1, bool digits = true, bool upper = true, bool lower = true, bool unique = false)
        {
            return Module.ModuleString.GeneratePRNG(length, number, digits, upper, lower, unique);
        }

        /// <summary>Generates random Vector2 with the C#-standard Pseudo-Random-Number-Generator.</summary>
        /// <param name="min">Smallest possible Vector2</param>
        /// <param name="max">Biggest possible Vector2</param>
        /// <param name="number">How many Vector2 you want to generate (default: 1, optional)</param>
        /// <returns>List with the generated Vector2.</returns>
        public static System.Collections.Generic.List<Vector2> GenerateVector2PRNG(Vector2 min, Vector2 max, int number = 1)
        {
            return Module.ModuleVector2.GeneratePRNG(min, max, number);
        }

        /// <summary>Generates random Vector3 with the C#-standard Pseudo-Random-Number-Generator.</summary>
        /// <param name="min">Smallest possible Vector3</param>
        /// <param name="max">Biggest possible Vector3</param>
        /// <param name="number">How many Vector3 you want to generate (default: 1, optional)</param>
        /// <returns>List with the generated Vector3.</returns>
        public static System.Collections.Generic.List<Vector3> GenerateVector3PRNG(Vector3 min, Vector3 max, int number = 1)
        {
            return Module.ModuleVector3.GeneratePRNG(min, max, number);
        }

        /// <summary>Generates random Vector4 with the C#-standard Pseudo-Random-Number-Generator.</summary>
        /// <param name="min">Smallest possible Vector4</param>
        /// <param name="max">Biggest possible Vector4</param>
        /// <param name="number">How many Vector4 you want to generate (default: 1, optional)</param>
        /// <returns>List with the generated Vector4.</returns>
        public static System.Collections.Generic.List<Vector4> GenerateVector4PRNG(Vector4 min, Vector4 max, int number = 1)
        {
            return Module.ModuleVector4.GeneratePRNG(min, max, number);
        }

        #endregion


        #region Private methods

        private static void logTRIsNull()
        {
            string errorMessage = "'instance' is null!" + System.Environment.NewLine + "Did you add the 'TrueRandom'-prefab to the current scene?";

            if (!loggedTRIsNull)
            {
                Debug.LogWarning(errorMessage);
                loggedTRIsNull = true;
            }
        }

        #endregion


        #region Event-trigger methods

        private void onGenerateIntegerStart()
        {
            generateCounter++;

            if (_onGenerateIntegerStart != null)
            {
                _onGenerateIntegerStart();
            }
        }

        private void onGenerateIntegerFinished(System.Collections.Generic.List<int> result, string id)
        {
            generateCounter--;

            if (_onGenerateIntegerFinished != null)
            {
                _onGenerateIntegerFinished(result, id);
            }
        }

        private void onGenerateFloatStart()
        {
            generateCounter++;

            if (_onGenerateFloatStart != null)
            {
                _onGenerateFloatStart();
            }
        }

        private void onGenerateFloatFinished(System.Collections.Generic.List<float> result, string id)
        {
            generateCounter--;

            if (_onGenerateFloatFinished != null)
            {
                _onGenerateFloatFinished(result, id);
            }
        }

        private void onGenerateSequenceStart()
        {
            generateCounter++;

            if (_onGenerateSequenceStart != null)
            {
                _onGenerateSequenceStart();
            }
        }

        private void onGenerateSequenceFinished(System.Collections.Generic.List<int> result, string id)
        {
            generateCounter--;

            if (_onGenerateSequenceFinished != null)
            {
                _onGenerateSequenceFinished(result, id);
            }
        }

        private void onGenerateStringStart()
        {
            generateCounter++;

            if (_onGenerateStringStart != null)
            {
                _onGenerateStringStart();
            }
        }

        private void onGenerateStringFinished(System.Collections.Generic.List<string> result, string id)
        {
            generateCounter--;

            if (_onGenerateStringFinished != null)
            {
                _onGenerateStringFinished(result, id);
            }
        }

        private void onGenerateVector2Start()
        {
            generateCounter++;

            if (_onGenerateVector2Start != null)
            {
                _onGenerateVector2Start();
            }
        }

        private void onGenerateVector2Finished(System.Collections.Generic.List<Vector2> result, string id)
        {
            generateCounter--;

            if (_onGenerateVector2Finished != null)
            {
                _onGenerateVector2Finished(result, id);
            }
        }

        private void onGenerateVector3Start()
        {
            generateCounter++;

            if (_onGenerateVector3Start != null)
            {
                _onGenerateVector3Start();
            }
        }

        private void onGenerateVector3Finished(System.Collections.Generic.List<Vector3> result, string id)
        {
            generateCounter--;

            if (_onGenerateVector3Finished != null)
            {
                _onGenerateVector3Finished(result, id);
            }
        }

        private void onGenerateVector4Start()
        {
            generateCounter++;

            if (_onGenerateVector4Start != null)
            {
                _onGenerateVector4Start();
            }
        }

        private void onGenerateVector4Finished(System.Collections.Generic.List<Vector4> result, string id)
        {
            generateCounter--;

            if (_onGenerateVector4Finished != null)
            {
                _onGenerateVector4Finished(result, id);
            }
        }

        private void onErrorInfo(string errorInfo)
        {
            if (_onErrorInfo != null)
            {
                _onErrorInfo(errorInfo);
            }
        }

        private void onUpdateQuota(int quota)
        {
            if (_onUpdateQuota != null)
            {
                _onUpdateQuota(quota);
            }
        }

        #endregion


        #region Editor-only methods

#if UNITY_EDITOR

        /// <summary>Generates random integers (Editor only).</summary>
        /// <param name="min">Smallest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="max">Biggest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="number">How many numbers you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        public static void GenerateIntegerInEditor(int min, int max, int number = 1)
        {
            if (Util.Helper.isEditorMode)
            {
                if (instance != null)
                {
                    System.Threading.Thread worker = new System.Threading.Thread(() => Module.ModuleInteger.GenerateInEditor(min, max, number, isPRNG));
                    worker.Start();
                }
                else
                {
                    logTRIsNull();
                }
            }
        }

        /// <summary>Generates random floats (Editor only).</summary>
        /// <param name="min">Smallest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="max">Biggest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="number">How many numbers you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        public static void GenerateFloatInEditor(float min, float max, int number = 1)
        {
            if (Util.Helper.isEditorMode)
            {
                if (instance != null)
                {
                    System.Threading.Thread worker = new System.Threading.Thread(() => Module.ModuleFloat.GenerateInEditor(min, max, number, isPRNG));
                    worker.Start();
                }
                else
                {
                    logTRIsNull();
                }
            }
        }

        /// <summary>Generates random sequence (Editor only).</summary>
        /// <param name="min">Start of the interval (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="max">End of the interval (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="number">How many numbers you have in the result (max range: max - min, optional)</param>
        public static void GenerateSequenceInEditor(int min, int max, int number = 0)
        {
            if (Util.Helper.isEditorMode)
            {
                if (instance != null)
                {
                    System.Threading.Thread worker = new System.Threading.Thread(() => Module.ModuleSequence.GenerateInEditor(min, max, number, isPRNG));
                    worker.Start();
                }
                else
                {
                    logTRIsNull();
                }
            }
        }

        /// <summary>Generates random strings (Editor only).</summary>
        /// <param name="length">How long the strings should be (range: 1 - 20)</param>
        /// <param name="number">How many strings you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        /// <param name="digits">Allow digits (0-9) (default: true, optional)</param>
        /// <param name="upper">Allow uppercase letters (default: true, optional)</param>
        /// <param name="lower">Allow lowercase letters (default: true, optional)</param>
        /// <param name="unique">String shoud be unique (default: false, optional)</param>
        public static void GenerateStringInEditor(int length, int number = 1, bool digits = true, bool upper = true, bool lower = true, bool unique = false)
        {
            if (Util.Helper.isEditorMode)
            {
                if (instance != null)
                {
                    System.Threading.Thread worker = new System.Threading.Thread(() => Module.ModuleString.GenerateInEditor(length, number, digits, upper, lower, unique, isPRNG));
                    worker.Start();
                }
                else
                {
                    logTRIsNull();
                }
            }
        }

        /// <summary>Generates random Vector2 (Editor only).</summary>
        /// <param name="min">Smallest possible Vector2 (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="max">Biggest possible Vector2 (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="number">How many Vector2 you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        public static void GenerateVector2InEditor(Vector2 min, Vector2 max, int number = 1)
        {
            if (Util.Helper.isEditorMode)
            {
                if (instance != null)
                {
                    System.Threading.Thread worker = new System.Threading.Thread(() => Module.ModuleVector2.GenerateInEditor(min, max, number, isPRNG));
                    worker.Start();
                }
                else
                {
                    logTRIsNull();
                }
            }
        }

        /// <summary>Generates random Vector3 (Editor only).</summary>
        /// <param name="min">Smallest possible Vector3 (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="max">Biggest possible Vector3 (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="number">How many Vector3 you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        public static void GenerateVector3InEditor(Vector3 min, Vector3 max, int number = 1)
        {
            if (Util.Helper.isEditorMode)
            {
                if (instance != null)
                {
                    System.Threading.Thread worker = new System.Threading.Thread(() => Module.ModuleVector3.GenerateInEditor(min, max, number, isPRNG));
                    worker.Start();
                }
                else
                {
                    logTRIsNull();
                }
            }
        }

        /// <summary>Generates random Vector4 (Editor only).</summary>
        /// <param name="min">Smallest possible Vector4 (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="max">Biggest possible Vector4 (range: -1'000'000'000 - 1'000'000'000)</param>
        /// <param name="number">How many Vector4 you want to generate (range: 1 - 10'000, default: 1, optional)</param>
        public static void GenerateVector4InEditor(Vector4 min, Vector4 max, int number = 1)
        {
            if (Util.Helper.isEditorMode)
            {
                if (instance != null)
                {
                    System.Threading.Thread worker = new System.Threading.Thread(() => Module.ModuleVector4.GenerateInEditor(min, max, number, isPRNG));
                    worker.Start();
                }
                else
                {
                    logTRIsNull();
                }
            }
        }

        /// <summary>Gets the remaining quota in bits from the server (Editor only).</summary>
        public static void GetQuotaInEditor()
        {
            if (Util.Helper.isEditorMode)
            {
                if (instance != null)
                {
                    System.Threading.Thread worker = new System.Threading.Thread(() => Module.ModuleQuota.GetQuotaInEditor());
                    worker.Start();
                }
                else
                {
                    logTRIsNull();
                }
            }
        }

#endif

        #endregion
    }
}
// © 2016-2018 crosstales LLC (https://www.crosstales.com)