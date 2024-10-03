using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void PressStartButton()
    {
        animator.SetTrigger("Close");
        animator.SetBool("Start",true);
    }
    public void PressHowToPlayButton()
    {
        animator.SetTrigger("Close");
        animator.SetBool("HowToPlay", true);
    }
    public void CloseHowToPlayButton()
    {
        animator.SetTrigger("Close");
        animator.SetBool("HowToPlay", false);
    }

    public void PressSettingsButton()
    {
        animator.SetTrigger("Close");
        animator.SetBool("Settings", true);
    }
    public void CloseSettingsButton()
    {
        animator.SetTrigger("Close");
        animator.SetBool("Settings", false);
    }
    public void SetScene(string sceneName)
    {
        SceneTransition.Instance.SetTransitionMode(ScreenTransitionManager.TransitionMode.WipeLeft);
        SceneTransition.Instance.TransitionToScene(sceneName);
    }
    public void SetSceneFade(string sceneName)
    {
        SceneTransition.Instance.SetTransitionMode(ScreenTransitionManager.TransitionMode.Fade);
        SceneTransition.Instance.TransitionToScene(sceneName);
    }

    public void ResumeGameAnimationComplete()
    {
        if (IngameUIManager.Instance)
        {
            if (animator.GetBool("HowToPlay"))
                return;
            if (animator.GetBool("Settings"))
                return;
            IngameUIManager.Instance.ResumeGameAnimationComplete();
        }
    }
    public bool IsAnimationName(string value)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(value);
    }
}
