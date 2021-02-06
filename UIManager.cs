using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private Camera _dummyCamera; //used for blocking view on loading scene

    public Events.EventFadeComplete OnMainMenuFadeComplete;

    private void Start()
    {
        _mainMenu.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    private void HandleMainMenuFadeComplete(bool fadeOut)
    {
        OnMainMenuFadeComplete?.Invoke(fadeOut);
    }

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        _pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
    }

    private void Update()
    {
        if(GameManager.Instance.CurrentGameState != GameManager.GameState.PREGAME)
            return;

        if(Input.GetKeyDown(KeyCode.Space))
            GameManager.Instance.StartGame();
    }

    public void SetDummyCameraActive(bool active)
    {
        _dummyCamera.gameObject.SetActive(active);
    }
}
