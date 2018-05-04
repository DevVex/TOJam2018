namespace Crosstales.TrueRandom.EditorUtil
{
    /// <summary>Collected editor constants of very general utility for the asset.</summary>
    public static class EditorConstants
    {

        #region Constant variables

        // Keys for the configuration of the asset
        public const string KEY_UPDATE_CHECK = Util.Constants.KEY_PREFIX + "UPDATE_CHECK";
        public const string KEY_REMINDER_CHECK = Util.Constants.KEY_PREFIX + "REMINDER_CHECK";
        public const string KEY_TELEMETRY = Util.Constants.KEY_PREFIX + "TELEMETRY";
        public const string KEY_PREFAB_AUTOLOAD = Util.Constants.KEY_PREFIX + "PREFAB_AUTOLOAD";

        public const string KEY_HIERARCHY_ICON = Util.Constants.KEY_PREFIX + "HIERARCHY_ICON";
        //public const string KEY_SHOW_QUOTA = KEY_PREFIX + "SHOW_QUOTA";

        public const string KEY_UPDATE_DATE = Util.Constants.KEY_PREFIX + "UPDATE_DATE";

        public const string KEY_REMINDER_DATE = Util.Constants.KEY_PREFIX + "REMINDER_DATE";
        public const string KEY_REMINDER_COUNT = Util.Constants.KEY_PREFIX + "REMINDER_COUNT";

        public const string KEY_LAUNCH = Util.Constants.KEY_PREFIX + "LAUNCH";

        public const string KEY_TELEMETRY_DATE = Util.Constants.KEY_PREFIX + "TELEMETRY_DATE";

        // Default values
        public const string DEFAULT_ASSET_PATH = "/Plugins/crosstales/TrueRandom/";
        public const bool DEFAULT_UPDATE_CHECK = true;
        public const bool DEFAULT_REMINDER_CHECK = true;
        public const bool DEFAULT_TELEMETRY = true;
        public const bool DEFAULT_PREFAB_AUTOLOAD = false;

        public const bool DEFAULT_HIERARCHY_ICON = true;
        //public const bool DEFAULT_SHOW_QUOTA = false;

        #endregion

            
        #region Changable variables

        /// <summary>Sub-path to the prefabs.</summary>
        public static string PREFAB_SUBPATH = "Prefabs/";

        #endregion


        #region Properties

        /// <summary>Returns the URL of the asset in UAS.</summary>
        /// <returns>The URL of the asset in UAS.</returns>
        public static string ASSET_URL
        {
            get
            {

                if (Util.Constants.isPro)
                {
                    return Util.Constants.ASSET_PRO_URL;
                }
                else
                {
                    return "https://www.assetstore.unity3d.com/#!/content/98709?aid=1011lNGT&pubref=" + Util.Constants.ASSET_NAME;
                }
            }
        }

        /// <summary>Returns the UID of the asset.</summary>
        /// <returns>The UID of the asset.</returns>
        public static System.Guid ASSET_UID
        {
            get
            {
                if (Util.Constants.isPro)
                {
                    return new System.Guid("20dba9ee-0be5-4d24-9427-c17b601499f9");
                }
                else
                {
                    return new System.Guid("4e5530d7-f7b5-494a-9112-ffbb49574c76");
                }
            }
        }
        
        #endregion
        
    }
}
// © 2016-2018 crosstales LLC (https://www.crosstales.com)