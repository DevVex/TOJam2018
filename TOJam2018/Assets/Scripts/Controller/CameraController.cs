using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{
    public class CameraController : MonoBehaviour
    {

        public System.Action<float> OnPositionUpdated;

        [SerializeField] BoxCollider _collider;
        [SerializeField] BoxCollider _killBox;
        [SerializeField] BoxCollider _killBoxTop;

        private Camera _camera;

        private float _lerpDuration = 1f;
        private float _lerpPositionValue = 0f;
        private float _lerpZoomValue = 0f;

        private float _minZoom = 10f;

        //tracking and zoom
        private Vector2 _ballPosition;  //position of tracked object
        private float _worldBottom; //bottom edge seen by camera
        private float _lead; //amount to lead target
        private float _above; //amount to see above target
        private float _minView; //lower limit of world unity seen by camera

        private float _ballCamDiff = 0.4f;
        private float _maxBallCamDiff = 0.40f;

        private Vector2 _offset = Vector2.zero;
        private float _worldBottomOffset = 1.75f;

        private Vector3 _startPosition;
        private Vector3 _oldPosition;
        private Vector3 _killboxPos = Vector3.zero;
        private Vector3 _killboxTopPos = Vector3.zero;

        private bool _track = false;
        private bool _ballMovingForward = true;

        #region SETUP
        private void Awake()
        {
            SetupVariables();
        }

        private void Start()
        {
            SubscribeToEvents();

            _track = true;
        }

        private void SetupVariables()
        {
            _startPosition = this.transform.position;
            _oldPosition = _startPosition;
            _camera = Camera.main;

            //_killboxPos = _killBox.transform.position;
            //_killboxTopPos = _killBoxTop.transform.position;

            //PositionCollider();
        }
        #endregion

        #region EVENTS
        private void SubscribeToEvents()
        {
            PlayerManager.Instance.Player.OnPlayerReset += Reset;

        }

        private void UnsubscribeToEvents()
        {
            if (PlayerManager.Instance)
            {
                if (PlayerManager.Instance.Player)
                {
                    PlayerManager.Instance.Player.OnPlayerReset -= Reset;
                }
            }
        }
        #endregion

        #region LOGIC
        private void Update()
        {
            if (GameManager.Instance.State == Constants.GameState.game || GameManager.Instance.State == Constants.GameState.launching)
            {
                if (PlayerManager.Instance)
                    _ballPosition = PlayerManager.Instance.FollowTarget.position;
            }
        }

        private void LateUpdate()
        {
            if (GameManager.Instance.State == Constants.GameState.game || GameManager.Instance.State == Constants.GameState.launching)
            {
                if (PlayerManager.Instance)
                {
                    if (_track == true)
                    {
                        //PositionCollider();

                        //update lerp values
                        _lerpPositionValue += Time.deltaTime / _lerpDuration;
                        _lerpZoomValue += Time.deltaTime / _lerpDuration;

                        float interpolation = (120f) * Time.deltaTime;


                        //update x if moving forward
                        Vector3 currentPos = this.transform.position;

                        float xTarget = currentPos.x;
                        if (BallIsAheadOfOffset() == true)
                            xTarget = Mathf.Max(this.transform.position.x, GetCamXPos());

                        if (PlayerManager.Instance.Player.MovingForward == true )
                            xTarget = GetCamMinXPos(xTarget);

                        //y position
                        float yTarget = this.transform.position.y;

                        currentPos.x = Mathf.Lerp(this.transform.position.x, Mathf.Max(this.transform.position.x, xTarget), interpolation);
                        currentPos.y = Mathf.Lerp(this.transform.position.y, yTarget, interpolation);

                        Vector3 targetPos = new Vector3(xTarget, this.transform.position.y, this.transform.position.z);

                        this.transform.position = currentPos;//Vector3.Lerp(this.transform.position, targetPos, Easing.Cubic.In(_lerpPositionValue));

                        if (OnPositionUpdated != null)
                            OnPositionUpdated(this.transform.position.x - _oldPosition.x);

                        _oldPosition = this.transform.position;
                    }
                }
            }
        }

        private float GetCamXPos()
        {
            Vector3 pos = _camera.WorldToViewportPoint(PlayerManager.Instance.FollowTarget.position);
            Vector3 posOffset = new Vector3(pos.x + _ballCamDiff, pos.y, pos.z);

            return _camera.ViewportToWorldPoint(posOffset).x;
        }

        private float GetCamMinXPos(float xTarget)
        {
            Vector3 ballPos = _camera.WorldToViewportPoint(PlayerManager.Instance.FollowTarget.position);
            Vector3 camPos = _camera.WorldToViewportPoint(this.transform.position);

            if ((camPos.x - ballPos.x) > _maxBallCamDiff)
            {
                Vector3 posOffset = new Vector3(ballPos.x + _maxBallCamDiff, ballPos.y, ballPos.z);

                return _camera.ViewportToWorldPoint(posOffset).x;
            }

            return xTarget;
        }

        private bool BallIsAheadOfOffset()
        {
            Vector3 ballPos = _camera.WorldToViewportPoint(PlayerManager.Instance.FollowTarget.position);
            Vector3 camPos = _camera.WorldToViewportPoint(this.transform.position);

            return (camPos.x - ballPos.x) < _ballCamDiff;
        }

        private void SetupOffset()
        {
            Vector2 screenPoint = _camera.ViewportToWorldPoint(Vector3.zero);

            float xo = PlayerManager.Instance.FollowTarget.position.x - screenPoint.x;
            float yo = this.transform.position.y - PlayerManager.Instance.FollowTarget.position.y;

            _offset = new Vector2(xo, yo);

            //tracking
            _worldBottom = _camera.ViewportToWorldPoint(Vector3.zero).y + _worldBottomOffset;
            _lead = _camera.WorldToViewportPoint(PlayerManager.Instance.FollowTarget.position).x;//_offset.x;
            _above = _offset.y;
            _minView = this.transform.position.y;
        }

        private void PositionCollider()
        {
            //wall
            Vector2 blockerPos = _camera.ViewportToWorldPoint(new Vector2(0f, 0.5f));
            _collider.gameObject.transform.position = new Vector2(blockerPos.x - (_collider.size.x / 1f), blockerPos.y);

            //killbox
            Vector2 killPos = _camera.ViewportToWorldPoint(new Vector2(0.5f, 0f));
            _killBox.gameObject.transform.position = new Vector2(killPos.x, _killboxPos.y);

            //top killbox
            _killBoxTop.gameObject.transform.position = new Vector2(killPos.x, _killboxTopPos.y);
        }
        #endregion

        #region CLEANUP
        private void OnDestroy()
        {
            UnsubscribeToEvents();
        }

        private void Reset()
        {
            _track = true;

            _lerpPositionValue = 0f;
            _lerpZoomValue = 0f;
            _camera.transform.position = _startPosition;
            _oldPosition = _startPosition;
        }
        #endregion
    }
}

