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
            if (collider.gameObject.tag == Constants.TAG_OBSTACLE)
            {
                ObstacleBase obstacle = collider.gameObject.GetComponent<ObstacleBase>();

                if(obstacle.BeenHit == false)
                {
                    obstacle.HitPlayer();
                    _playerRef.ChangeSpeed(Constants.GetSpeedForObstacle(obstacle.Type));
                }
                
            }
        }

        virtual protected void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.tag == Constants.TAG_GROUND)
            {
                _playerRef.HitGround();
            }
        }

        virtual protected void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag == Constants.TAG_GROUND)
            {
                _playerRef.LeftGround();
            }
        }

    }
}
