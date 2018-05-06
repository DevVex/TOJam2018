using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{
    public class ObstacleBase : MonoBehaviour
    {

        [SerializeField] protected Constants.ObstacleType _type;
        public Constants.ObstacleType Type { get { return _type; } }

        protected bool _beenHit;
        public bool BeenHit { get { return _beenHit; } }

        protected void OnEnable()
        {
            _beenHit = false;
        }


        public void HitPlayer ()
        {
            _beenHit = true;
        }
    }
}

