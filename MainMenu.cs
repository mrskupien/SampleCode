using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animation _mainMenuAnimator;
    [SerializeField] private AnimationClip _fadeOutAnimation; //clips with events OnFadeOutComplete and OnFadeInComplete
    [SerializeField] private AnimationClip _fadeInAnimation;

    public Events.EventFadeComplete OnMainMenuFadeComplete;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    public void OnFadeOutComplete()
    {
        Debug.Log("FadeOut Complete");

        OnMainMenuFadeComplete?.Invoke(true);
        
    }
    public void OnFadeInComplete()
    {
        Debug.Log("FadeIn Complete");

        OnMainMenuFadeComplete?.Invoke(false);
        UIManager.Instance.SetDummyCameraActive(true);
    }

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if(previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
            FadeOut();
        if(previousState != GameManager.GameState.PREGAME && currentState == GameManager.GameState.PREGAME)
            FadeIn();
    }

    private void FadeIn()
    {
        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeInAnimation;
        _mainMenuAnimator.Play();
    }
    private void FadeOut()
    {
        UIManager.Instance.SetDummyCameraActive(false);

        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeOutAnimation;
        _mainMenuAnimator.Play();
    }
}
