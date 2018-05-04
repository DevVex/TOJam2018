using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.TrueRandom.Util;

namespace Crosstales.TrueRandom.Demo
{
    /// <summary>Generate random Vector2.</summary>
    [HelpURL("https://www.crosstales.com/media/data/assets/truerandom/api/class_crosstales_1_1_true_random_1_1_demo_1_1_generate_vector2.html")]
    public class GenerateVector2 : MonoBehaviour
    {
        #region Variables

        public GameObject TextPrefab;

        public GameObject ScrollView;
        public InputField Number;
        public InputField MinX;
        public InputField MinY;
        public InputField MaxX;
        public InputField MaxY;
        public Text Error;
        public Text Quota;

        #endregion


        #region MonoBehaviour methods

        public void OnEnable()
        {
            TRManager.OnGenerateVector2Start += onGenerateVector2Start;
            TRManager.OnGenerateVector2Finished += onGenerateVector2Finished;
            TRManager.OnUpdateQuota += onUpdateQuota;
            TRManager.OnErrorInfo += onError;

            if (Quota != null)
                Quota.text = "Quota: " + TRManager.CurrentQuota;
        }

        public void OnDisable()
        {
            TRManager.OnGenerateVector2Start -= onGenerateVector2Start;
            TRManager.OnGenerateVector2Finished -= onGenerateVector2Finished;
            TRManager.OnUpdateQuota -= onUpdateQuota;
            TRManager.OnErrorInfo -= onError;
        }

        #endregion


        #region Public methods

        public void GenerateVector2Numbers()
        {
            if (Error != null)
                Error.text = string.Empty;
            //Error.text = "Generating... Please wait";

            float minX;
            float maxX;
            float minY;
            float maxY;
            int number;

            if (Number != null && int.TryParse(Number.text, out number))
            {
                if (MinX != null && float.TryParse(MinX.text, out minX))
                {
                    if (MaxX != null && float.TryParse(MaxX.text, out maxX))
                    {
                        if (MinY != null && float.TryParse(MinY.text, out minY))
                        {
                            if (MaxY != null && float.TryParse(MaxY.text, out maxY))
                            {
                                TRManager.GenerateVector2(new Vector2(minX, minY), new Vector2(maxX, maxY), number);
                            }
                            else
                            {
                                if (Error != null)
                                    Error.text = "'Max Y value' is not a number!";
                            }
                        }
                        else
                        {
                            if (Error != null)
                                Error.text = "'Min Y value' is not a number!";
                        }
                    }
                    else
                    {
                        if (Error != null)
                            Error.text = "'Max X value' is not a number!";
                    }
                }
                else
                {
                    if (Error != null)
                        Error.text = "'Min X value' is not a number!";
                }
            }
            else
            {
                if (Error != null)
                    Error.text = "'Number of Vector2' is not a number!";
            }
        }

        #endregion


        #region Callbacks

        private void onUpdateQuota(int e)
        {
            if (Quota != null)
                Quota.text = "Quota: " + e;
        }

        private void onGenerateVector2Start()
        {
            if (Config.DEBUG)
                Debug.Log("Start generating Vector2");
        }

        private void onError(string e)
        {
            if (Error != null)
                Error.text = e;
        }

        private void onGenerateVector2Finished(List<Vector2> e, string id)
        {
            for (int ii = ScrollView.transform.childCount - 1; ii >= 0; ii--)
            {
                Transform child = ScrollView.transform.GetChild(ii);
                child.SetParent(null);
                Destroy(child.gameObject);
            }

            if (Config.DEBUG)
                Debug.Log("Finished generating Vector2: " + id);

            ScrollView.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 80 * e.Count);

            for (int ii = 0; ii < e.Count; ii++)
            {
                if (Config.DEBUG)
                    Debug.Log(e[ii]);

                GameObject go = Instantiate(TextPrefab);

                go.transform.SetParent(ScrollView.transform);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = new Vector3(10, -80 * ii, 0);
                go.GetComponent<Text>().text = e[ii].x + ", " + e[ii].y;
            }
        }

        #endregion
    }
}
// © 2017-2018 crosstales LLC (https://www.crosstales.com)