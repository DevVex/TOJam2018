using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.TrueRandom.Util;

namespace Crosstales.TrueRandom.Demo
{
    /// <summary>Generate a random sequence.</summary>
    [HelpURL("https://www.crosstales.com/media/data/assets/truerandom/api/class_crosstales_1_1_true_random_1_1_demo_1_1_generate_sequence.html")]
    public class GenerateSequence : MonoBehaviour
    {
        #region Variables

        public GameObject TextPrefab;

        public GameObject ScrollView;
        public InputField Min;
        public InputField Max;
        public InputField Number;

        public Text Error;
        public Text Quota;

        #endregion


        #region MonoBehaviour methods

        public void OnEnable()
        {
            TRManager.OnUpdateQuota += onUpdateQuota;
            TRManager.OnGenerateSequenceStart += onGenerateSequenceStart;
            TRManager.OnErrorInfo += onError;
            TRManager.OnGenerateSequenceFinished += onGenerateSequenceFinished;

            if (Quota != null)
                Quota.text = "Quota: " + TRManager.CurrentQuota;
        }

        public void OnDisable()
        {
            TRManager.OnUpdateQuota -= onUpdateQuota;
            TRManager.OnGenerateSequenceStart -= onGenerateSequenceStart;
            TRManager.OnErrorInfo -= onError;
            TRManager.OnGenerateSequenceFinished -= onGenerateSequenceFinished;
        }

        #endregion


        #region Public methods

        public void GenerateSeq()
        {
            if (Error != null)
                Error.text = string.Empty;

            int min;
            int max;
            int number = 0;

            if (Min != null && int.TryParse(Min.text, out min))
            {

                if (Max != null && int.TryParse(Max.text, out max))
                {
                    int.TryParse(Number.text, out number);

                    TRManager.GenerateSequence(min, max, number);
                }
                else
                {
                    if (Error != null)
                        Error.text = "'Max sequence value' is not a number!";
                }
            }
            else
            {
                if (Error != null)
                    Error.text = "'Min sequence value' is not a number!";
            }
        }

        #endregion


        #region Callbacks

        private void onUpdateQuota(int e)
        {
            if (Quota != null)
                Quota.text = "Quota: " + e;
        }

        private void onGenerateSequenceStart()
        {
            if (Config.DEBUG)
                Debug.Log("Start generating sequence");
        }

        public void onError(string e)
        {
            if (Error != null)
                Error.text = e;
        }

        private void onGenerateSequenceFinished(List<int> e, string id)
        {
            for (int ii = ScrollView.transform.childCount - 1; ii >= 0; ii--)
            {
                Transform child = ScrollView.transform.GetChild(ii);
                child.SetParent(null);
                Destroy(child.gameObject);
            }

            if (Config.DEBUG)
                Debug.Log("Finished generating sequence: " + id);

            ScrollView.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 80 * e.Count);

            for (int ii = 0; ii < e.Count; ii++)
            {
                if (Config.DEBUG)
                    Debug.Log(e[ii]);

                GameObject go = Instantiate(TextPrefab);
                go.transform.SetParent(ScrollView.transform);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = new Vector3(10, -80 * ii, 0);
                go.GetComponent<Text>().text = e[ii].ToString();
            }
        }

        #endregion
    }
}
// © 2016-2018 crosstales LLC (https://www.crosstales.com)