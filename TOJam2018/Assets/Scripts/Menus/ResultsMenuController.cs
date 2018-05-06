using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TOJAM
{
    public class ResultsMenuController : MenuBase
    {
        [SerializeField] private TextMeshProUGUI _finalText;
        [SerializeField] private TextMeshProUGUI _bestText;
        [SerializeField] protected Button _replayButton;

        private float _launchDistance;
        private const string _resultsPrefix = "You launched your boi ";
        private const string _resultsSuffix = " cool meters! \n RADICAL! ";

        private const string _newBest = "THATS A NEW RECORD! Your boi is proud";
        private const string _failedBest = "Someone launched your boi ";
        private const string _failedBestEnd = " cool meters though. Try harder next time.";

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
            float best = CheckBest(_launchDistance);

            //your score text
            _finalText.text = _resultsPrefix + _launchDistance.ToString("F1") + _resultsSuffix;

            //best text
            if(_launchDistance <= best)
            {
                _bestText.text = _failedBest + best.ToString("F1") + _failedBestEnd;
            }
            else
            {
                PlayerPrefs.SetFloat(Constants.PLAYER_PREFS_BEST, _launchDistance);
                _bestText.text = _newBest;
            }

            base.Show();
        }

        private float CheckBest (float score)
        {
            if(PlayerPrefs.HasKey(Constants.PLAYER_PREFS_BEST))
            {
                return (PlayerPrefs.GetFloat(Constants.PLAYER_PREFS_BEST));
            }
            else
            {
                PlayerPrefs.SetFloat(Constants.PLAYER_PREFS_BEST, score);
                return score;
            }
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

