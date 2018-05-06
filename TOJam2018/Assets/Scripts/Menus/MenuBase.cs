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
        
        //_BW TODO left off here
        //need to link this to main menu and menu manager
        //enable this menu on start
        //tap to start
        //then make results menu and store top scores?
    }
}

