using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TOJAM
{
    public class ResultsMenuController : MenuBase
    {
        [SerializeField] protected Button _replayButton;

        private float _launchDistance;
        private string _resultsPrefix = "You launched your boi ";
        private string _resultsSuffix = " cool meters! RADICAL! ";

        public System.Action OnReplay;

        new protected void Awake()
        {
            base.Awake();

            _replayButton.onClick.AddListener(OnPlayClicked);
        }

        protected void OnDestroy()
        {
            _replayButton.onClick.RemoveListener(OnPlayClicked);
        }

        override public void Show()
        {
            _launchDistance = PlayerManager.Instance.Player.DistanceCovered;

            base.Show();
        }

        public void OnPlayClicked()
        {
            if (_buttonLock == false)
            {
                Hide();

                if(LoadingManager.Instance == null)
                {
                    GameObject go = new GameObject();
                    go.AddComponent<LoadingManager>();
                }


                LoadingManager.Instance.LoadScene(Constants.SCENE_PRELOADER);

                if (OnReplay != null)
                    OnReplay.Invoke();
            }
        }
    }
}

