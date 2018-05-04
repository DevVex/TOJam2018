using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.TrueRandom.Util;

namespace Crosstales.TrueRandom.Demo
{
    /// <summary>Simulates n random dices with the values 1-6.</summary>
    [HelpURL("https://www.crosstales.com/media/data/assets/truerandom/api/class_crosstales_1_1_true_random_1_1_demo_1_1_dice_roll.html")]
    public class DiceRoll : MonoBehaviour
    {
        #region Variables

        public GameObject TextPrefab;

        public GameObject ScrollView;
        public InputField Number;
        public Text Error;
        public Text Quota;

        #endregion


        #region MonoBehaviour methods

        public void OnEnable()
        {
            TRManager.OnUpdateQuota += onUpdateQuota;
            TRManager.OnGenerateIntegerStart += onGenerateIntegerStart;
            TRManager.OnErrorInfo += onError;
            TRManager.OnGenerateIntegerFinished += onGenerateIntegerFinished;

            if (Quota != null)
                Quota.text = "Quota: " + TRManager.CurrentQuota;
        }

        public void OnDisable()
        {
            TRManager.OnUpdateQuota -= onUpdateQuota;
            TRManager.OnGenerateIntegerStart -= onGenerateIntegerStart;
            TRManager.OnErrorInfo -= onError;
            TRManager.OnGenerateIntegerFinished -= onGenerateIntegerFinished;
        }

        #endregion


        #region Public methods

        public void SimulateRoll()
        {
            if (Error != null)
                Error.text = string.Empty;

            int number;

            if (Number != null && int.TryParse(Number.text, out number))
            {
                TRManager.GenerateInteger(1, 6, number);
            }
            else
            {
                if (Error != null)
                    Error.text = "'Number of dices' is not a number!";
            }
        }

        #endregion


        #region Callbacks

        private void onUpdateQuota(int e)
        {
            if (Quota != null)
                Quota.text = "Quota: " + e;
        }

        private void onGenerateIntegerStart()
        {
            if (Config.DEBUG)
                Debug.Log("Start simulating dice rolls");
        }

        private void onError(string e)
        {
            if (Error != null)
                Error.text = e;
        }

        private void onGenerateIntegerFinished(List<int> e, string id)
        {
            for (int ii = ScrollView.transform.childCount - 1; ii >= 0; ii--)
            {
                Transform child = ScrollView.transform.GetChild(ii);
                child.SetParent(null);
                Destroy(child.gameObject);
            }

            if (Config.DEBUG)
                Debug.Log("Finished simulating dice rolls: " + id);

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