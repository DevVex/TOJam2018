using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{

    public class MenuBase : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected Constants.MenuType _type;

        public Constants.MenuType Type { get { return _type; } }
        

        protected bool _buttonLock = false;

        virtual protected void Awake()
        {
            Hide();
        }

        virtual public void Show()
        {
            if(_animator.gameObject.activeInHierarchy)
            {
                _buttonLock = false;

                _animator.SetBool(Constants.ANIMATOR_SHOW_TRIGGER, true);
                //_animator.SetBool(BoatRockerConstants.STOP_TRIGGER, false);
            }
        }

        virtual public void Hide()
        {
            if(_animator.gameObject.activeInHierarchy)
            {
                _buttonLock = true;

                _animator.SetBool(Constants.ANIMATOR_SHOW_TRIGGER, false);
               // _animator.SetBool(BoatRockerConstants.STOP_TRIGGER, true);
            }
        }
    }
}

