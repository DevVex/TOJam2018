using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{
    public class Player : MonoBehaviour
    {
        public System.Action OnPlayerReset;

        [SerializeField] private Rigidbody2D _cartRigidbody;
        [SerializeField] private Rigidbody2D _personRigidbody;

        public bool MovingForward { get { return _cartRigidbody.velocity.x > 0f; } }

        //speed
        private float _speed = 10f;
        private float _minSpeed = 10f;
        private float _maxSpeed = 100f;
        private float _speedGain = 0.1f;

        private float _jumpForce = 15f;
        private float _jumpStrength = 0f;

        private float _minJumpForce = 0f;
        private float _maxJumpForce = 375f;

        private float _jumpTime = 0.3f;
        private float _jumpTimeBase = 0.3f;

        private bool _canJump = true;
        private bool _increaseJump = false;


        //player should gain speed while on ground
        //jump based on time click touched for short and high jumps
        //hit obstacles to gain or lose speed

        // Use this for initialization
        private void Awake()
        {
            SetupVariables();
        }

        void Start()
        {
            SubscribeToEvents();
        }

        private void SetupVariables ()
        {
            _cartRigidbody.bodyType = RigidbodyType2D.Kinematic;
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

        private void HandleGameStateChanged(Constants.GameState state)
        {
            if (state == Constants.GameState.game)
            {
                _cartRigidbody.bodyType = RigidbodyType2D.Dynamic;
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.State == Constants.GameState.game)
            {
                UpdateSpeed();
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

            return _cartRigidbody.velocity.y;
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

        private void UpdateSpeed()
        {
            if (_canJump == true)
            {
                _speed += _speedGain;
            }
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
            velX = Mathf.Clamp(_speed, _minSpeed, _maxSpeed);

            if (Input.GetMouseButtonDown(0))
            {                
                velY = Jump();
            }

            if (Input.GetMouseButtonUp(0))
            {
                StopJump();
            }

            if (_canJump == false)
            {
                _cartRigidbody.AddForce(Vector2.up * (_jumpStrength * (_jumpTime)), ForceMode2D.Force);
            }

            //update velocity
            _cartRigidbody.velocity = new Vector2(velX,velY);
        }

        #region COLLISION
        public void HitGround ()
        {
            _canJump = true;
        }

        public void LeftGround ()
        {
            //_canJump = false;
        }
        #endregion

    }
}

