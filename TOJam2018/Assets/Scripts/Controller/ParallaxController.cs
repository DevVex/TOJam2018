using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{
    public class ParallaxController : MonoBehaviour
    {
 
            [SerializeField] private SpritePool _spritePool;
            [SerializeField] private Sprite[] _groundSprites;

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

            private float _xPosSeamOffset = 0.009f;

            private List<SpriteRenderer> _activeSprites = new List<SpriteRenderer>();
            private List<SpriteRenderer> _activeSpritesClone = new List<SpriteRenderer>();
            private Queue<SpriteRenderer> _spritesToAdd = new Queue<SpriteRenderer>();

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

                Reset();
            }

            private void SetupVariables()
            {
                _spritePool.Init();

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

            }

            private void Move(float cameraDelta)
            {
                if (GameManager.Instance.State == Constants.GameState.game || GameManager.Instance.State == Constants.GameState.launching)
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
                    this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(target, this.transform.position.y, this.transform.position.z), _lerpPositionValue);
                }
            }

            public void SetupEnvironment()
            {
                //check next flag
                float worldScreenHeight = Camera.main.orthographicSize * 2f;
                float worldScreenWidth = (worldScreenHeight / Screen.height) * Screen.width;

                //calculate pieces needed
                SpriteRenderer s = _spritePool.GetPooledObject();
                s.sprite = AdvanceGroundPiece();
                _piecesNeeded = Mathf.CeilToInt(worldScreenWidth / s.bounds.size.x) + _extraPieces;
                _activeSprites.Add(s);

                //place first piece
                _spriteWidth = GetWidestSprite();//s.bounds.size.x;
                float offset = (s.bounds.size.x / 2f);
                float xPos = Camera.main.ViewportToWorldPoint(Vector2.zero).x + offset + _initialOffset;
                float yPos = this.transform.position.y;
                s.transform.position = new Vector2(xPos, yPos);

                float xEdgeFirstPiece = 0f;
                _piecesUsed++;

                //populate with X extra
                for (int i = 0; i < _piecesNeeded; i++)
                {
                    SpriteRenderer temp = _spritePool.GetPooledObject();
                    temp.sprite = AdvanceGroundPiece();

                    temp.transform.position = new Vector2(xPos + ((offset * 2f) * (i + 1)), yPos);
                    temp.transform.position += new Vector3((_xSpacing * i) - _xPosSeamOffset, Random.Range(-_ySpacing, _ySpacing), 0f);
                    _activeSprites.Add(temp);

                    _piecesUsed++;
                }

            }

            private float GetWidestSprite()
            {
                float width = 0f;

                foreach (Sprite s in _groundSprites)
                {
                    if (s.bounds.size.x > width)
                        width = s.bounds.size.x;
                }

                return width;
            }

            private Sprite AdvanceGroundPiece()
            {
                if (_followOrder == true)
                {
                    Sprite s = _groundSprites[_groundTracker];

                    _groundTracker++;

                    if (_groundTracker >= _groundSprites.Length)
                        _groundTracker = 0;

                    return s;
                }
                else
                {
                    int num = Mathf.RoundToInt(Random.value * 100f);

                    if (num % 2 == 0)
                    {
                        Sprite s = _groundSprites[_groundTracker];

                        _groundTracker++;

                        if (_groundTracker >= _groundSprites.Length)
                            _groundTracker = 0;

                        return s;
                    }

                    return _groundSprites[0];
                }
            }

            private void PoolGroundPiece(SpriteRenderer s)
            {
                _spritePool.ReturnPooledObject(s);
            }

            private void Update()
            {
                if (GameManager.Instance.State == Constants.GameState.game || GameManager.Instance.State == Constants.GameState.launching)
                {
                    CheckPiecesNeeded();

                    //Remove pieces
                    //List<SpriteRenderer> sprites = new List<SpriteRenderer>(_activeSprites);
                    _activeSpritesClone.Clear();
                    _activeSpritesClone.AddRange(_activeSprites);

                    for (int j = 0; j < _activeSpritesClone.Count; j++)
                    {
                        SpriteRenderer s = _activeSpritesClone[j];

                        if (s.gameObject.transform.position.x < (Camera.main.ScreenToWorldPoint(Vector2.zero).x - (_spriteWidth * (_extraPieces / 2f))))
                        {
                            _lastPiecePosition = s.transform.position;
                            _activeSprites.Remove(s);
                            PoolGroundPiece(s);
                            QueueEnvironmentPiece();
                        }
                    }

                    //Add new pieces
                    if (_spritesToAdd.Count > 0)
                    {
                        int counter = _spritesToAdd.Count;
                        for (int i = 0; i < counter; i++)
                        {
                            SpriteRenderer temp = _spritesToAdd.Dequeue();
                            temp.sprite = AdvanceGroundPiece();

                            Vector2 basePos = Vector2.zero;
                            if (_activeSprites.Count > 0)
                                basePos = _activeSprites[_activeSprites.Count - 1].transform.position;
                            else
                                basePos = new Vector2(_lastPiecePosition.x, _lastPiecePosition.y);

                            temp.transform.position = basePos + new Vector2(_spriteWidth, 0f);
                            temp.transform.position += new Vector3(_xSpacing - _xPosSeamOffset, Random.Range(-_ySpacing, _ySpacing), 0f);
                            _activeSprites.Add(temp);

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
                if (_activeSprites.Count < _piecesNeeded)
                    _spritesToAdd.Enqueue(_spritePool.GetPooledObject());
            }

            private void Reset()
            {
                _activeSpritesClone.Clear();
                _activeSpritesClone.AddRange(_activeSprites);

                foreach (SpriteRenderer s in _activeSpritesClone)
                {
                    _activeSprites.Remove(s);
                    PoolGroundPiece(s);
                }

                _activeSprites.Clear();
                _spritesToAdd.Clear();

                this.transform.position = _startPosition;
                _piecesUsed = 0;

                SetupEnvironment();
            }
                   
    }

}
