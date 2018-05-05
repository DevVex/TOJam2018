using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{
    public class GameManager : MonoBehaviour
    {
        public Constants.GameState State { get; protected set; }

        public System.Action<Constants.GameState> OnGameStateChanged;

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
        }

        private void Start()
        {
            StartCoroutine(StartGame());
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


    }
}

