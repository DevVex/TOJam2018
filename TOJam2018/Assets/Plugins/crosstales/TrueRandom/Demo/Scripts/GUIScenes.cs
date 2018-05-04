using UnityEngine;
using UnityEngine.SceneManagement;

namespace Crosstales.TrueRandom.Demo
{
    /// <summary>Main GUI scene manager for all demo scenes.</summary>
    [HelpURL("https://www.crosstales.com/media/data/assets/truerandom/api/class_crosstales_1_1_true_random_1_1_demo_1_1_g_u_i_scenes.html")]
    public class GUIScenes : MonoBehaviour
    {

        #region Variables

        [Tooltip("Name of the previous scene.")]
        public string PreviousScene;

        [Tooltip("Name of the next scene.")]
        public string NextScene;

        public void LoadPrevoiusScene()
        {
            SceneManager.LoadScene(PreviousScene);
        }

        #endregion


        #region Public methods

        public void LoadNextScene()
        {
            SceneManager.LoadScene(NextScene);
        }

        #endregion
    }
}
// © 2016-2018 crosstales LLC (https://www.crosstales.com)