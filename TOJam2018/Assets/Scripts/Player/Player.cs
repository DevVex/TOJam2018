using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{
    public class Player : MonoBehaviour
    {

        [SerializeField] private Rigidbody2D _cartRigidbody;
        [SerializeField] private Rigidbody2D _personRigidbody;

        private float _speed = 1f;
        private float _jumpForce = 30f;
        private float _jumpStrength = 0f;

        private float _jumpMod = 500f;
        private float _minJumpForce = 0f;
        private float _maxJumpForce = 100f;

        private float _jumpTime = 0.3f;
        private float _jumpTimeBase = 0.3f;

        private bool _canJump = true;
        private bool _increaseJump = false;


        //player should gain speed while on ground
        //jump based on time click touched for short and high jumps
        //hit obstacles to gain or lose speed

        // Use this for initialization
        void Start()
        {

        }

        private void SubscribeToEvents ()
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }

        private void UnsubscribeToEvents()
        {
            if(GameManager.Instance)
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }

        private void HandleGameStateChanged (Constants.GameState state)
        {

        }


        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.State == Constants.GameState.game)
            {
                UpdateJump();
                UpdateControls();
                
                
            }           

        }

        private float Jump()
        {
            if(_canJump == true)
            {
                _canJump = false;
                _increaseJump = true;
                _jumpStrength = _maxJumpForce;
                return _jumpForce;
            }

            return 0f;
        }

        private void StopJump ()
        {
            _increaseJump = false;
            _jumpStrength = _minJumpForce;
            _jumpTime = _jumpTimeBase;
        }

        private void ChangeSpeed(float speedChange)
        {
            
        }

        private void UpdateJump ()
        {
            if(_canJump == false)
            {
                if (Input.GetMouseButton(0))
                {
                    if (_jumpTime > 0f && _increaseJump == true )
                    {
                        _jumpTime -= Time.deltaTime;
                    }
                    else
                    {
                        StopJump();
                    }
                }

            }
        }

        private void UpdateControls()
        {
            float velX = _cartRigidbody.velocity.x;
            float velY = _cartRigidbody.velocity.y;

            //temp update speed
            //velX = _speed;

            if (Input.GetMouseButtonDown(0))
            {
                Jump();
               // velY = _jumpForce;
            }

            if (Input.GetMouseButtonUp(0))
            {
                StopJump();
            }

            if (_canJump == false)
            {
                _cartRigidbody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Force);
            }

            //update velocity
            _cartRigidbody.velocity = new Vector2(velX,velY);
        }

        #region COLLISION
        public void HitGround ()
        {
            _canJump = true;
        }
        #endregion

    }
}

