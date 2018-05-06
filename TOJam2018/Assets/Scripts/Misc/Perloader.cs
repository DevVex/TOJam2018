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
            StartCoroutine(LoadGame());
        }

        private IEnumerator LoadGame()
        {

            yield return new WaitForSeconds(2f);

            LoadingManager.Instance.LoadScene(Constants.SCENE_GAMEPLAY);
        }

    }
}

