namespace Crosstales.TrueRandom.Util
{
    /// <summary>Configuration for the asset.</summary>
    public static class Config
    {

        #region Changable variables

        /// <summary>Enable or disable debug logging for the asset.</summary>
        public static bool DEBUG = Constants.DEFAULT_DEBUG;

        /// <summary>Don't destroy the objects during scene switches.</summary>
        //public static bool DONT_DESTROY_ON_LOAD = Constants.DEFAULT_DONT_DESTROY_ON_LOAD;

        /// <summary>Shows the quota inside the editor components.</summary>
        public static bool SHOW_QUOTA = Constants.DEFAULT_SHOW_QUOTA;

        /// <summary>Is the configuration loaded?</summary>
        public static bool isLoaded = false;

        #endregion


        #region Public static methods

        /// <summary>Resets all changable variables to their default value.</summary>
        public static void Reset()
        {
            DEBUG = Constants.DEFAULT_DEBUG;
            //DONT_DESTROY_ON_LOAD = Constants.DEFAULT_DONT_DESTROY_ON_LOAD;
            SHOW_QUOTA = Constants.DEFAULT_SHOW_QUOTA;
        }

        /// <summary>Loads the all changable variables.</summary>
        public static void Load()
        {
            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_DEBUG))
            {
                DEBUG = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_DEBUG);
            }

            //if (CTPlayerPrefs.HasKey(Constants.KEY_DONT_DESTROY_ON_LOAD))
            //{
            //    DONT_DESTROY_ON_LOAD = CTPlayerPrefs.GetBool(Constants.KEY_DONT_DESTROY_ON_LOAD);
            //}

            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_SHOW_QUOTA))
            {
                SHOW_QUOTA = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_SHOW_QUOTA);
            }

            isLoaded = true;
        }

        /// <summary>Saves the all changable variables.</summary>
        public static void Save()
        {
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_DEBUG, DEBUG);
            //CTPlayerPrefs.SetBool(Constants.KEY_DONT_DESTROY_ON_LOAD, DONT_DESTROY_ON_LOAD);
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_SHOW_QUOTA, SHOW_QUOTA);

            Common.Util.CTPlayerPrefs.Save();
        }

        #endregion

    }
}
// © 2017-2018 crosstales LLC (https://www.crosstales.com)