using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{
    public class PlatformBase : MonoBehaviour
    {
        [SerializeField] Collider2D _groundCollider;

        public Collider2D GroundCollider { get { return _groundCollider; } }

        [HideInInspector] public Constants.PlatformDifficulty Difficulty;
        [HideInInspector] public PlatformPool PlatformPoolRef;
    }
}

