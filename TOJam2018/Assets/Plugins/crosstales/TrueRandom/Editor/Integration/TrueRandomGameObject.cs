using UnityEngine;
using UnityEditor;
using Crosstales.TrueRandom.EditorUtil;

namespace Crosstales.TrueRandom.EditorIntegration
{
    /// <summary>Editor component for the "Hierarchy"-menu.</summary>
	public class TrueRandomGameObject : MonoBehaviour
    {

        [MenuItem("GameObject/" + Util.Constants.ASSET_NAME + "/" + Util.Constants.TRUERANDOM_SCENE_OBJECT_NAME, false, EditorHelper.GO_ID)]
        private static void AddTrueRandom()
        {
            EditorHelper.InstantiatePrefab(Util.Constants.TRUERANDOM_SCENE_OBJECT_NAME);
            GAApi.Event(typeof(TrueRandomGameObject).Name, "Add " + Util.Constants.TRUERANDOM_SCENE_OBJECT_NAME);
        }

        [MenuItem("GameObject/" + Util.Constants.ASSET_NAME + "/" + Util.Constants.TRUERANDOM_SCENE_OBJECT_NAME, true)]
        private static bool AddTrueRandomValidator()
        {
            return !EditorHelper.isTrueRandomInScene;
        }
    }
}
// © 2017-2018 crosstales LLC (https://www.crosstales.com)
