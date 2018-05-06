using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM 
{
    public class MenuManager : MonoBehaviour
    {       
        [SerializeField] private MainMenuController _mainMenuController;
        [SerializeField] private ResultsMenuController _resultsController;

        private MenuBase _currentMenu;
        private Stack<Constants.MenuType> _previousMenus = new Stack<Constants.MenuType>();

        private bool _firstMenuShow = true;

        void Start()
        {
            SubscribeToEvents();
        }

        #region EVENTS
        private void SubscribeToEvents ()
        {
            if (GameManager.Instance)
                GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }

        private void UnsubscribeToEvents ()
        {
            if (GameManager.Instance)
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
        #endregion

        void OnDestroy()
        {
            UnsubscribeToEvents();
        }

        private void OnBack()
        {
            RemoveListeners(_currentMenu.Type);
            StartCoroutine(AddPreviousListeners());
        }

        private IEnumerator AddPreviousListeners()
        {
            //            Debug.Log("AddPreviousListeners " + _previousMenus.Count);
            yield return new WaitForEndOfFrame();

            switch(_previousMenus.Pop())
            {
                case Constants.MenuType.main:
                    _currentMenu = _mainMenuController;
                    _mainMenuController.OnPlay += Load;
                    break;
                default:
                    break;
            }

            _currentMenu.Show();
            //            Debug.Log("AddPreviousListeners " + _previousMenus.Count);
        }

        private void RemoveListeners(Constants.MenuType fromMenu, bool hideMenu = true)
        {
            if(hideMenu == true)
                _currentMenu.Hide();

            switch(fromMenu)
            {
                case Constants.MenuType.main:
                    _mainMenuController.OnPlay -= Load;
                    break;
                default:
                    break;
            }
        }

        #region SHOW_MENUS
        private void ShowMainMenu ()
        {
            _currentMenu = _mainMenuController;
            _mainMenuController.OnPlay += Load;

            _currentMenu.Show();
        }

        private void ShowResultsMenu ()
        {
            _currentMenu = _resultsController;

            _currentMenu.Show();
        }


        private void HideCurrent ()
        {
            if (_currentMenu != null)
            {
                _currentMenu.Hide();
                RemoveListeners(_currentMenu.Type);

                foreach (Constants.MenuType menu in _previousMenus)
                {
                    RemoveListeners(menu, false);
                }
                _previousMenus.Clear();
            }
        }
        #endregion

        #region LOGIC
        private void Load()
        {
            _mainMenuController.OnPlay -= Load;
            StartCoroutine(StartGame());
        }

        private IEnumerator StartGame()
        {
            Resources.UnloadUnusedAssets();

            yield return new WaitForEndOfFrame();

            GameManager.Instance.SetGameState(Constants.GameState.game);
            //LoadingManager.Instance.LoadScene(_sceneToLoad.ToString());
        }

        private void OnQuit()
        {
            Application.Quit();
        }

        private void HandleGameStateChanged (Constants.GameState state)
        {
            if (state == Constants.GameState.game)
            {
                
            }
            else if (state == Constants.GameState.menu)
            {
                ShowMainMenu();
            }
            else if (state == Constants.GameState.gameOver)
            {
                ShowResultsMenu();
            }
            else if (state == Constants.GameState.launching)
            {
               
            }
            else
            {

            }
        }
        #endregion
    }
}
