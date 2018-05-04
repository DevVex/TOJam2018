using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.TrueRandom.Util;

namespace Crosstales.TrueRandom.Demo
{
    /// <summary>Generate random strings.</summary>
    [HelpURL("https://www.crosstales.com/media/data/assets/truerandom/api/class_crosstales_1_1_true_random_1_1_demo_1_1_generate_strings.html")]
    public class GenerateStrings : MonoBehaviour
    {
        #region Variables

        public GameObject TextPrefab;

        public GameObject ScrollView;
        public InputField Number;
        public InputField Length;
        public Toggle Digits;
        public Toggle UppercaseLetters;
        public Toggle LowecaseLetters;
        public Toggle Unique;
        public Text Error;
        public Text Quota;

        #endregion


        #region MonoBehaviour methods

        public void OnEnable()
        {
            TRManager.OnUpdateQuota += onUpdateQuota;
            TRManager.OnGenerateStringStart += onGenerateStringStart;
            TRManager.OnErrorInfo += onError;
            TRManager.OnGenerateStringFinished += onGenerateStringFinished;

            if (Quota != null)
                Quota.text = "Quota: " + TRManager.CurrentQuota;
        }

        public void OnDisable()
        {
            TRManager.OnUpdateQuota -= onUpdateQuota;
            TRManager.OnGenerateStringStart -= onGenerateStringStart;
            TRManager.OnErrorInfo -= onError;
            TRManager.OnGenerateStringFinished -= onGenerateStringFinished;
        }

        #endregion


        #region Public methods

        public void GenerateString()
        {
            if (Error != null)
                Error.text = string.Empty;

            int number;
            int length;

            if (Number != null && int.TryParse(Number.text, out number))
            {
                if (Length != null && int.TryParse(Length.text, out length))
                {
                    TRManager.GenerateString(length, number, Digits.isOn, UppercaseLetters.isOn, LowecaseLetters.isOn, Unique.isOn);
                }
                else
                {
                    if (Error != null)
                        Error.text = "'Length of the strings' is not a number!";
                }
            }
            else
            {
                if (Error != null)
                    Error.text = "'Number of strings' is not a number!";
            }
        }

        #endregion


        #region Callbacks

        private void onUpdateQuota(int e)
        {
            if (Quota != null)
                Quota.text = "Quota: " + e;
        }

        private void onGenerateStringStart()
        {
            if (Config.DEBUG)
                Debug.Log("Start generating strings");
        }

        private void onError(string e)
        {
            if (Error != null)
                Error.text = e;
        }

        private void onGenerateStringFinished(List<string> e, string id)
        {
            for (int ii = ScrollView.transform.childCount - 1; ii >= 0; ii--)
            {
                Transform child = ScrollView.transform.GetChild(ii);
                child.SetParent(null);
                Destroy(child.gameObject);
            }

            if (Config.DEBUG)
                Debug.Log("Finished generating string: " + id);

            ScrollView.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 80 * e.Count);

            for (int ii = 0; ii < e.Count; ii++)
            {
                if (Config.DEBUG)
                    Debug.Log(e[ii]);

                GameObject go = Instantiate(TextPrefab);
                go.transform.SetParent(ScrollView.transform);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = new Vector3(10, -80 * ii, 0);
                go.GetComponent<Text>().text = e[ii];
            }
        }

        #endregion
    }
}
// © 2016-2018 crosstales LLC (https://www.crosstales.com)