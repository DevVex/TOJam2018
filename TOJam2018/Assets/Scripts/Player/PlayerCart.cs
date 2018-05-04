using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{
    public class PlayerCart : MonoBehaviour
    {
        [SerializeField] protected Player _playerRef;

        virtual protected void OnTriggerEnter2D (Collider2D collider)
        {

        }

        virtual protected void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.tag == Constants.TAG_GROUND)
            {
                _playerRef.HitGround();
            }
        }
    }
}
