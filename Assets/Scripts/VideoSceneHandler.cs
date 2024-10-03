using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoSceneHandler : MonoBehaviour
{
    [SerializeField]
    VideoPlayer videoPlayer;
    bool finished = false;
    [SerializeField]
    Button skipButton;
    private void Start()
    {
        skipButton.gameObject.SetActive(false);
        StartCoroutine(EnableSkipButton());
    }
    IEnumerator EnableSkipButton()
    {
        yield return new WaitForSeconds(5.0f);
        if (skipButton)
            skipButton.gameObject.SetActive(true);
        yield return null;
    }
    private void Update()
    {
        if (videoPlayer != null && finished == false)
        {
            if (!videoPlayer.isPlaying && videoPlayer.frame > 0)
            {
                finished = true;
                SceneTransition.Instance.SetTransitionMode(ScreenTransitionManager.TransitionMode.Fade);
                SceneTransition.Instance.TransitionToScene("Whiteboxed");
            }
        }
    }

    public void SkipButtonPress()
    {
        if (finished == false)
        {
            finished = true;
            SceneTransition.Instance.SetTransitionMode(ScreenTransitionManager.TransitionMode.Fade);
            SceneTransition.Instance.TransitionToScene("Whiteboxed");
        }
    }
}
