using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{
    public class Perloader : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            LoadGame();
        }

        private void LoadGame()
        {
            LoadingManager.Instance.LoadScene(Constants.SCENE_GAMEPLAY);
        }

    }
}

