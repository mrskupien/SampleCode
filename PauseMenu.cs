using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button ResumeButton;
    [SerializeField] private Button RestartButton;
    [SerializeField] private Button QuitButton;

    private void Start()
    {
        ResumeButton.onClick.AddListener(HandleResumeClicked);
        RestartButton.onClick.AddListener(HandlRestartClicked);
        QuitButton.onClick.AddListener(HandleQuitClicked);
    }

    private void HandleResumeClicked()
    {
        Debug.Log("Resume clicked");
        GameManager.Instance.TooglePause();
    }
    private void HandlRestartClicked()
    {
        Debug.Log("Restart clicked");
        GameManager.Instance.RestartGame();
    }
    private void HandleQuitClicked()
    {
        Debug.Log("Quit clicked");
        GameManager.Instance.QuitGame();
    }
}
