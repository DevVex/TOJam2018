using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TOJAM 
{

    public class MainMenuController : MenuBase {

        [SerializeField] protected Button _playButton;

        public System.Action OnPlay;

        new protected void Awake()
        {
            base.Awake();

            _playButton.onClick.AddListener(OnPlayClicked);
        }

        protected void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OnPlayClicked);
        }

        public void OnPlayClicked()
        {
            if (_buttonLock == false)
            {
                Hide();

                if (OnPlay != null)
                    OnPlay.Invoke();    
            }
        }
            
    }

}
