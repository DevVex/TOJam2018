using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{
    public class PlatformManager : MonoBehaviour
    {
        private static PlatformManager _instance;
        public static PlatformManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = GameObject.FindObjectOfType<PlatformManager>();

                return _instance;
            }
        }

        [Range(0f, 1f)]
        [SerializeField]
        private float _parallaxSpeed;

        [SerializeField] private float _xSpacing;
        [SerializeField] private float _ySpacing;

        [SerializeField] private float _initialOffset = 0f;

        [SerializeField] private bool _followOrder = true;
        [SerializeField] private bool _variableWidth = false;

        [SerializeField] private bool _alwaysMove = false;
        [Range(0f, 10f)]
        [SerializeField]
        private float _nonGameMovementSpeed;

        private float _lerpDuration = 1f;
        private float _lerpPositionValue = 0f;

        private int _groundTracker = 0;

        private float _xPosSeamOffset = 0.001f;

        private List<PlatformBase> _activePlatforms = new List<PlatformBase>();
        private List<PlatformBase> _activePlatformsClone = new List<PlatformBase>();
        private Queue<PlatformBase> _platformsToAdd = new Queue<PlatformBase>();

        private float _spriteWidth = 0f;

        private int _piecesNeeded = 0;
        [SerializeField] private int _extraPieces = 2;

        private int _piecesUsed = 0;

        private Vector3 _lastPiecePosition = Vector3.zero;
        private Vector3 _startPosition = Vector3.zero;

        private void Awake()
        {
            SetupVariables();
        }

        // Use this for initialization
        private void Start()
        {
            SubscribeToEvents();
        }

        private void SetupVariables()
        {
            _startPosition = this.transform.position;
        }

        private void SubscribeToEvents()
        {
            if (GameManager.Instance)
                GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;

            CameraController cam = Camera.main.GetComponent<CameraController>();
            if (cam != null)
                cam.OnPositionUpdated += Move;
        }

        private void UnsubscribeToEvents()
        {
            if (GameManager.Instance)
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;

            CameraController cam = Camera.main.GetComponent<CameraController>();
            if (cam != null)
                cam.OnPositionUpdated -= Move;
        }

        private void HandleGameStateChanged(Constants.GameState state)
        {
            if (state == Constants.GameState.game)
                SetupEnvironment();
        }

        private void Move(float cameraDelta)
        {
            if (GameManager.Instance.State == Constants.GameState.game)
            {
                //update lerp values
                _lerpPositionValue += Time.deltaTime / _lerpDuration;

                //update target
                float nonMovetarget = -(_nonGameMovementSpeed * _parallaxSpeed);
                float moveTarget = (cameraDelta * _parallaxSpeed);

                float offset = 0f;

                //choose target based on layer type
                if (Mathf.Abs(nonMovetarget) > Mathf.Abs(moveTarget) && _alwaysMove == true)
                    offset = nonMovetarget;
                else
                    offset = moveTarget;

                float target = this.transform.position.x + offset;
                this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(target, this.transform.position.y, this.transform.position.z),_lerpPositionValue);
            }
        }

        public void SetupEnvironment()
        {
            //check next flag
            float worldScreenHeight = Camera.main.orthographicSize * 2f;
            float worldScreenWidth = (worldScreenHeight / Screen.height) * Screen.width;

            //calculate pieces needed
            PlatformBase go = ObjectPoolManager.Instance.GetPooledObject(Constants.PlatformDifficulty.none);
            _piecesNeeded =  Mathf.CeilToInt(worldScreenWidth / go.GroundCollider.bounds.size.x) + _extraPieces;
            _activePlatforms.Add(go);

            //place first piece
            _spriteWidth = go.GroundCollider.bounds.size.x;
            float offset = (go.GroundCollider.bounds.size.x / 2f);
            float xPos = Camera.main.ViewportToWorldPoint(Vector2.zero).x + offset + _initialOffset;
            float yPos = this.transform.position.y;
            go.transform.position = new Vector2(xPos, yPos);

            _piecesUsed++;

            //populate with X extra
            for (int i = 0; i < _piecesNeeded; i++)
            {
                PlatformBase temp = ObjectPoolManager.Instance.GetPooledObject(Constants.PlatformDifficulty.none);

                temp.transform.position = new Vector2(xPos + ((offset * 2f) * (i + 1)), yPos);
                temp.transform.position += new Vector3((_xSpacing * i) - _xPosSeamOffset, Random.Range(-_ySpacing, _ySpacing), 0f);
                _activePlatforms.Add(temp);

                _piecesUsed++;
            }

        }

        private void PoolGroundPiece(PlatformBase go)
        {
            ObjectPoolManager.Instance.ReturnPooledObject( go);
        }

        private void Update()
        {
            if (GameManager.Instance.State == Constants.GameState.game)
            {
                CheckPiecesNeeded();

                //Remove pieces
                //List<SpriteRenderer> sprites = new List<SpriteRenderer>(_activeSprites);
                _activePlatformsClone.Clear();
                _activePlatformsClone.AddRange(_activePlatforms);

                for (int j = 0; j < _activePlatformsClone.Count; j++)
                {
                    PlatformBase go = _activePlatformsClone[j];

                    if (go.transform.position.x < (Camera.main.ScreenToWorldPoint(Vector2.zero).x - (_spriteWidth * (_extraPieces / 2f))))
                    {
                        _lastPiecePosition = go.transform.position;
                        _activePlatforms.Remove(go);
                        PoolGroundPiece(go);
                        QueueEnvironmentPiece();
                    }
                }

                //Add new pieces
                if (_platformsToAdd.Count > 0)
                {
                    int counter = _platformsToAdd.Count;
                    for (int i = 0; i < counter; i++)
                    {
                        PlatformBase temp = _platformsToAdd.Dequeue();

                        Vector2 basePos = Vector2.zero;
                        if (_activePlatforms.Count > 0)
                            basePos = _activePlatforms[_activePlatforms.Count - 1].transform.position;
                        else
                            basePos = new Vector2(_lastPiecePosition.x, _lastPiecePosition.y);

                        temp.transform.position = basePos + new Vector2(_spriteWidth, 0f);
                        temp.transform.position += new Vector3(_xSpacing - _xPosSeamOffset, Random.Range(-_ySpacing, _ySpacing), 0f);
                        _activePlatforms.Add(temp);

                        _piecesUsed++;
                    }
                }
            }
            else
            {
                if (_alwaysMove == true)
                {
                    //update lerp values
                    _lerpPositionValue += Time.deltaTime / _lerpDuration;

                    //update if moving forward
                    float target = this.transform.position.x - (_nonGameMovementSpeed * _parallaxSpeed);
                    this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(target, this.transform.position.y, this.transform.position.z),_lerpPositionValue);
                }
            }
        }

        private void CheckPiecesNeeded()
        {
            float worldScreenHeight = Camera.main.orthographicSize * 2f;
            float worldScreenWidth = (worldScreenHeight / Screen.height) * Screen.width;

            int needed = Mathf.CeilToInt(worldScreenWidth / _spriteWidth) + _extraPieces;

            if (_piecesNeeded < needed)
            {
                int diff = needed - _piecesNeeded;
                _piecesNeeded = needed;

                for (int i = 0; i < diff; i++)
                    QueueEnvironmentPiece();
            }
            else if (_piecesNeeded > needed)
            {
                _piecesNeeded = needed;
            }
        }

        private void QueueEnvironmentPiece()
        {
            if (_activePlatforms.Count < _piecesNeeded)
                _platformsToAdd.Enqueue(ObjectPoolManager.Instance.GetPooledObject(Constants.PlatformDifficulty.easy));
        }

        private void Reset()
        {
            _activePlatformsClone.Clear();
            _activePlatformsClone.AddRange(_activePlatforms);

            foreach (PlatformBase go in _activePlatformsClone)
            {
                _activePlatforms.Remove(go);
                PoolGroundPiece(go);
            }

            _activePlatforms.Clear();
            _platformsToAdd.Clear();

            this.transform.position = _startPosition;
            _piecesUsed = 0;

            SetupEnvironment();
        }
    }
}

