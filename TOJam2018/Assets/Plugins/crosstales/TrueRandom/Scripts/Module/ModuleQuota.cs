using UnityEngine;

namespace Crosstales.TrueRandom.Module
{
    /// <summary>
    /// This module gets the remaining quota on www.random.org.
    /// </summary>
    public abstract class ModuleQuota : BaseModule
    {

        #region Variables

        private static int quota = 1000000;

        #endregion


        #region Static properties

        /// <summary>Returns the remaining quota in bits from the last check.</summary>
        /// <returns>Remaining quota in bits from the last check.</returns>
        public static int Quota
        {
            get
            {
                return quota;
            }
        }

        #endregion


        #region Events

        public delegate void UpdateQuota(int quota);

        /// <summary>Event to get a message with the current quota.</summary>
        public static event UpdateQuota OnUpdateQuota
        {
            add { _onUpdateQuota += value; }
            remove { _onUpdateQuota -= value; }
        }

        private static UpdateQuota _onUpdateQuota;

        #endregion


        #region Public methods

        /// <summary>Gets the remaining quota in bits from the server.</summary>
        public static System.Collections.IEnumerator GetQuota()
        {
            if (Util.Helper.isInternetAvailable)
            {
                using (WWW www = new WWW(Util.Constants.GENERATOR_URL + "quota/?format=plain"))
                {

                    do
                    {
                        yield return www;
                    } while (!www.isDone);

                    if (string.IsNullOrEmpty(www.error))
                    {
                        if (!int.TryParse(www.text, out quota))
                        {
                            Debug.LogError("Could not parse value to integer: " + www.text);
                        }

                        onUpdateQuota(quota);
                    }
                    else
                    {
                        onErrorInfo(www.error);
                        Debug.LogWarning("Could not read from url: " + www.error);
                    }
                }
            }
            else
            {
                string msg = "No Internet access available - can't get quota!";
                Debug.LogError(msg);
                onErrorInfo(msg);

                quota = 1000000;
            }
        }

        #endregion


        #region Private methods

        private static void onUpdateQuota(int quota)
        {
            if (Util.Config.DEBUG)
                Debug.Log("onUpdateQuota: " + quota);

            if (_onUpdateQuota != null)
            {
                _onUpdateQuota(quota);
            }
        }

        #endregion


        #region Editor-only methods

#if UNITY_EDITOR

        /// <summary>Gets the remaining quota in bits from the server (Editor only).</summary>
        public static void GetQuotaInEditor()
        {
#if !UNITY_WSA
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = Util.Helper.RemoteCertificateValidationCallback;

                using (System.Net.WebClient client = new Common.Util.CTWebClient())
                {
                    using (System.IO.Stream stream = client.OpenRead(Util.Constants.GENERATOR_URL + "quota/?format=plain"))
                    {
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                        {

                            string content = reader.ReadToEnd();

                            if (Util.Config.DEBUG)
                                Debug.Log(content);

                            if (!int.TryParse(content, out quota))
                            {
                                Debug.LogWarning("Could not parse quota!: " + content);
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex);
            }
#endif
        }

#endif

        #endregion
    }
}
// © 2016-2018 crosstales LLC (https://www.crosstales.com)
