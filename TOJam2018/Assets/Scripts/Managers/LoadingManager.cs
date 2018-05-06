using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace TOJAM
{
    public class LoadingManager : MonoBehaviour
    {
        [SerializeField] private Animator _loadingAnimator;
        private bool _isLoading = false;
        private bool _authenticated = false;

        private static LoadingManager _instance;
        public static LoadingManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<LoadingManager>();
                }
                return _instance;
            }
        }

        void Awake()
        {
            Instance.Setup();

            if (Instance != this)
            {
                Destroy(this.gameObject);
            }

            else
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }

        void Setup()
        {
            _isLoading = false;



        }

        public void ShowLoading()
        {
            _isLoading = true;

            if (_loadingAnimator.gameObject.activeInHierarchy)
            {
                //_loadingAnimator.SetBool(BoatRockerConstants.LOADING_TRIGGER, true);
            }
        }

        public void HideLoading()
        {
            if (_loadingAnimator.gameObject.activeInHierarchy)
            {
                //_loadingAnimator.SetBool(BoatRockerConstants.LOADING_TRIGGER, false);
            }

            _isLoading = false;
        }

        public void LoadScene(string sceneName, bool initialLoad = false)
        {
            if (_isLoading == false)
            {
                StartCoroutine(LoadSceneAsync(sceneName, initialLoad));
            }
        }

        private IEnumerator LoadSceneAsync(string sceneName, bool initialLoad = false)
        {
            ShowLoading();

            yield return new WaitForSeconds(2f);

            yield return SceneManager.LoadSceneAsync(sceneName);

            yield return new WaitForEndOfFrame();

            HideLoading();
        }

        #region RESET

        public void ShowReset()
        {
            _isLoading = true;

            if (_loadingAnimator.gameObject.activeInHierarchy)
            {
                //_loadingAnimator.SetBool(BoatRockerConstants.RESET_TRIGGER, true);
            }
        }

        public void HideReset()
        {
            if (_loadingAnimator.gameObject.activeInHierarchy)
            {
                //_loadingAnimator.SetBool(BoatRockerConstants.RESET_TRIGGER, false);
            }

            _isLoading = false;
        }

        public void ResetScene(bool adWillPlay = false)
        {
            if (_isLoading == false)
                StartCoroutine(ResetSceneAsync(adWillPlay));
        }

        private IEnumerator ResetSceneAsync(bool adWillPlay)
        {
            ShowReset();

            //if (adWillPlay == false)
            //    yield return new WaitForSeconds(BoatRockerConstants.RESET_SCENE_DELAY);
            //else
            //    yield return new WaitForSeconds(BoatRockerConstants.RESET_SCENE_DELAY * 4f);

            yield return new WaitForEndOfFrame();

            HideReset();
        }

        #endregion
    }
}
