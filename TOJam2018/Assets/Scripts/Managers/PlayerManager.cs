using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] Player _player;
        [SerializeField] Transform _followTarget;
        [SerializeField] Transform _launchFollowTarget;

        private bool _followBody = false;

        public Player Player { get { return _player; } }
        public Transform FollowTarget
        { get
            {
                if (_followBody == true)
                    return _launchFollowTarget;
                else
                    return _followTarget;
            }
        }

        private static PlayerManager _instance;
        public static PlayerManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = GameObject.FindObjectOfType<PlayerManager>();

                return _instance;
            }
        }

        public void SetFollowBody (bool followBody)
        {
            _followBody = followBody;
        }
    }
}

