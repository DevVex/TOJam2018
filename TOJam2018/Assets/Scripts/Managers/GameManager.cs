using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{
    public class GameManager : MonoBehaviour
    {
        public Constants.GameState State { get; protected set; }

        public System.Action<Constants.GameState> OnGameStateChanged;

        private int _numPlatforms = 0;

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
                //SetGameState(Constants.GameState.launching);
            }
        }

        private IEnumerator StartGame ()
        {
            yield return new WaitForEndOfFrame();

            SetGameState(Constants.GameState.game);
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
            if (_numPlatforms < Constants.GAME_END_NUM_PLATFORMS)
            {
                return Constants.PlatformDifficulty.easy;
            }
            else
                return Constants.PlatformDifficulty.none;
        }
    }
}

