using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using Crosstales.TrueRandom.EditorUtil;

namespace Crosstales.TrueRandom.EditorTask
{
    /// <summary>Automatically adds the neccessary TrueRandom-prefabs to the current scene.</summary>
    [InitializeOnLoad]
    public class AutoInitalize
    {

        #region Variables

        private static Scene currentScene;

        #endregion


        #region Constructor

        static AutoInitalize()
        {
            EditorApplication.hierarchyWindowChanged += hierarchyWindowChanged;
        }

        #endregion


        #region Private static methods

        private static void hierarchyWindowChanged()
        {
            if (currentScene != EditorSceneManager.GetActiveScene())
            {
                if (EditorConfig.PREFAB_AUTOLOAD)
                {
                    if (!EditorHelper.isTrueRandomInScene)
                        EditorHelper.InstantiatePrefab(Util.Constants.TRUERANDOM_SCENE_OBJECT_NAME);
                }

                currentScene = EditorSceneManager.GetActiveScene();
            }
        }

        #endregion
    }
}
// © 2016-2018 crosstales LLC (https://www.crosstales.com)