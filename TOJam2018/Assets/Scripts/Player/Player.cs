using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{
    public class Player : MonoBehaviour
    {
        public System.Action OnPlayerReset;

        [SerializeField] private Rigidbody2D _cartRigidbody;

        [SerializeField] private GameObject _bodyParent;
        [SerializeField] private Rigidbody2D _personBody;
        private Rigidbody2D[] _bodyParts;

        private SpriteRenderer[] _playerSprites;

        public bool MovingForward { get { return _cartRigidbody.velocity.x > 0f; } }
        public float DistanceCovered { get { return _personBody.transform.position.x - _launchStart; } }

        //speed
        private float _speed = 1f;
        private float _minSpeed = 1f;
        private float _maxSpeed = 100f;
        private float _speedGain = 0.1f;

        //jumps
        [SerializeField]  private float _jumpForce = 15f;
        private float _jumpStrength = 0f;

        private float _minJumpForce = 0f;
        [SerializeField]  private float _maxJumpForce = 375f;

        private float _jumpTime = 0.3f;
        private float _jumpTimeBase = 0.3f;

        //hit flash
        private float _hitTime = 1f;
        private float _hitTimeBase = 1f;
        private float _flashSpeed = 10f;

        //launch 
        private float _maxLaunchForce = 2000f;

        private float _launchStart = 0f;

        private bool _canJump = true;
        public bool CanJump {  get { return _canJump; } }
        private bool _increaseJump = false;

        private bool _canGetHit = true;
        private bool _checkEndGame = false;

        private bool _launched = false;
        public bool HasLaunched { get { return _launched; } }


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
            _speed = _minSpeed;

            _playerSprites = this.GetComponentsInChildren<SpriteRenderer>();
            SetupBody();
        }

        private void SetupBody()
        {
            _bodyParts = _bodyParent.transform.GetComponentsInChildren<Rigidbody2D>();
            
            foreach (Rigidbody2D rigidBody in _bodyParts)
            {
                rigidBody.isKinematic = true;
            }
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
                UpdateSprites();

            }
            else if (GameManager.Instance.State == Constants.GameState.launching)
            {
                UpdatePreLaunch();
            }
        }

        private void Launch()
        {
            _launched = true;
            _launchStart = _personBody.transform.position.x;
            PlayerManager.Instance.SetFollowBody(true);

            //cameraTarget.SetTarget(body.gameObject);
            foreach (Rigidbody2D rigidBody in _bodyParts)
            {
                rigidBody.isKinematic = false;
            }

            float launchPower = (_speed / _maxSpeed) * _maxLaunchForce; 
            _personBody.AddForce(new Vector2(50.0f, 20.0f) * launchPower);

            StartCoroutine(CheckForEndGame());
        }

        private IEnumerator CheckForEndGame ()
        {
            yield return new WaitForSeconds(1f);

            _checkEndGame = true;
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

        public void ChangeSpeed(Constants.ObstacleType type)
        {
            if(type == Constants.ObstacleType.barrier)
            {
                _cartRigidbody.velocity = Vector3.zero;
                _cartRigidbody.bodyType = RigidbodyType2D.Kinematic;
                Launch();
            }
            else if (type == Constants.ObstacleType.runway)
            {
                GameManager.Instance.SetGameState(Constants.GameState.launching);
            }
            else
            {
                if (_canGetHit == true)
                {
                    _speed += Constants.GetSpeedForObstacle(type);

                    if (type != Constants.ObstacleType.oil)
                    {
                        _hitTime = _hitTimeBase;
                        _canGetHit = false;
                    }
                }
            }
                      
                
        }

        private void UpdateSprites()
        {
            if(_canGetHit == false)
            {
                //timeout can get hit
                if (_hitTime > 0f)
                {
                    _hitTime -= Time.deltaTime;

                    foreach(SpriteRenderer sprite in _playerSprites)
                    {
                        float alpha = Mathf.PingPong(Time.time * _flashSpeed, 0.5f) + 0.5f;
                        sprite.color = new Color(1f, 1f, 1f, alpha);
                    }
                }
                else
                {
                    _canGetHit = true;

                    foreach (SpriteRenderer sprite in _playerSprites)
                    {
                        sprite.color = new Color(1f, 1f, 1f, 1f);
                    }
                }
            }
        }

        private void UpdateSpeed()
        {
            if (_canJump == true && _canGetHit == true)
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
            _speed = Mathf.Clamp(_speed, _minSpeed, _maxSpeed);
            velX = _speed;

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
            _cartRigidbody.velocity = new Vector2(Mathf.Max(_minSpeed, velX),velY);
        }

        private void UpdatePreLaunch ()
        {
            if(_cartRigidbody.bodyType != RigidbodyType2D.Kinematic)
            {
                float velX = _cartRigidbody.velocity.x;
                float velY = _cartRigidbody.velocity.y;

                //temp update speed
                _speed = Mathf.Clamp(_speed, _minSpeed, _maxSpeed);
                velX = _speed;

                //update velocity
                _cartRigidbody.velocity = new Vector2(Mathf.Max(_minSpeed, velX), velY);
            }

            if(_checkEndGame == true)
            {
                if(_personBody.velocity.x <= 0f)
                {
                    GameManager.Instance.SetGameState(Constants.GameState.gameOver);
                }
            }
        }

        #region COLLISION
        public void HitGround ()
        {
            if(_canJump == false)
                _canJump = true;
            else
            {
                //getting stuck bug fix
                _cartRigidbody.AddForce((Vector2.up * 75f), ForceMode2D.Force);
            }
        }

        public void LeftGround ()
        {
            //_canJump = false;
        }
        #endregion

    }
}

