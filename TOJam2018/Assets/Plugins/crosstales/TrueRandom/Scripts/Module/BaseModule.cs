using UnityEngine;

namespace Crosstales.TrueRandom.Module
{
    public abstract class BaseModule
    {
        #region Variables

        //protected static readonly System.Random rnd = new System.Random ();

        //protected static bool isRunning = false;

        #endregion


        #region Events

        public delegate void ErrorInfo(string error);

        /// <summary>Event to get a message when an error occured.</summary>
        public static event ErrorInfo OnErrorInfo
        {
            add { _onErrorInfo += value; }
            remove { _onErrorInfo -= value; }
        }

        private static ErrorInfo _onErrorInfo;

        #endregion


        #region Private methods

        protected static void onErrorInfo(string errorInfo)
        {
            if (Util.Config.DEBUG)
                Debug.Log("onErrorInfo: " + errorInfo);

            if (_onErrorInfo != null)
            {
                _onErrorInfo(errorInfo);
            }
        }

        #endregion
    }
}
// © 2016-2018 crosstales LLC (https://www.crosstales.com)