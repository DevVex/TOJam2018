using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TOJAM
{
    [System.Serializable]
    public class DifficultyChunk
    {
        public int Weight;
        [SerializeField]
        public List<Constants.PlatformDifficulty> Chunk = new List<Constants.PlatformDifficulty>();
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField] protected DifficultyChunk[] _chunks;

        public Constants.GameState State { get; protected set; }
        public System.Action<Constants.GameState> OnGameStateChanged;

        private int _numPlatforms = 0;
        private List<Constants.PlatformDifficulty> _levelChunks = new List<Constants.PlatformDifficulty>();
        private Queue<Constants.PlatformDifficulty> _levelChunkQueue = new Queue<Constants.PlatformDifficulty>();

        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = GameObject.FindObjectOfType<GameManager>();

                return _instance;
            }
        }

        private void Awake()
        {
            Instance.SetupVariables();
        }

        private void SetupVariables()
        {
            ObjectPoolManager.Instance.Init();

            _numPlatforms = 0;
        }

        private void Start()
        {
            SubscribeToEvents();
            SetupLevel();

            StartCoroutine(StartGame());
        }

        private void SubscribeToEvents ()
        {
            PlatformManager.Instance.OnSpawnedPlatform += HandlePlatformSpawned;
        }

        private void UnsubscribeToEvents()
        {
            if(PlatformManager.Instance)
                PlatformManager.Instance.OnSpawnedPlatform -= HandlePlatformSpawned;
        }

        private void HandlePlatformSpawned ()
        {
            if (_numPlatforms < Constants.GAME_END_NUM_PLATFORMS)
                _numPlatforms++;
            else
            {
                SetGameState(Constants.GameState.launching);
            }
        }

        private void SetupLevel()
        {
            //added to weighted Dict
            Dictionary<DifficultyChunk, int> weightedChunks = new Dictionary<DifficultyChunk, int>();
            
            foreach(DifficultyChunk chunk in _chunks)
            {
                weightedChunks.Add(chunk, chunk.Weight);
            }

            //generate level
            while(_levelChunks.Count < Constants.GAME_END_NUM_PLATFORMS)
            {
                _levelChunks.AddRange(WeightedRandomizer.From(weightedChunks).TakeOne().Chunk);
            }

            //add end of level
            List<Constants.PlatformDifficulty> temp = new List<Constants.PlatformDifficulty>() { Constants.PlatformDifficulty.none, Constants.PlatformDifficulty.none, Constants.PlatformDifficulty.none, Constants.PlatformDifficulty.none, Constants.PlatformDifficulty.end};
            _levelChunks.AddRange(temp);

            //add to queue
            _levelChunkQueue = new Queue<Constants.PlatformDifficulty>(_levelChunks);
        }

        private IEnumerator StartGame ()
        {
            yield return new WaitForEndOfFrame();

            SetGameState(Constants.GameState.menu);
        }

        public void SetGameState(Constants.GameState state)
        {
            if(state != State)
            {
                State = state;

                if (OnGameStateChanged != null)
                    OnGameStateChanged(State);
            }
        }

        public Constants.PlatformDifficulty GetNextPlatformSet()
        {
            if (_levelChunkQueue.Count > 0)
            {
                return _levelChunkQueue.Dequeue();
            }
            else
                return Constants.PlatformDifficulty.none;
        }
    }
}

