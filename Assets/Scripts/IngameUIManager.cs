    using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameUIManager : MonoBehaviour
{
    bool isPaused = false;
    bool isDead = false;
    [SerializeField]
    GameObject pauseUI;
    public Entity playerEntity;
    [SerializeField]
    MainMenuHandler mainMenuHandle;
    public static IngameUIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        PlayerHack playerHack = FindAnyObjectByType<PlayerHack>();
        if (playerHack)
            playerEntity = playerHack.GetComponent<Entity>();
    }
    public void RetryScene()
    {
        ChangeScene(SceneManager.GetActiveScene().name);
    }
    public void ChangeScene(string sceneName)
    {
        SceneTransition.Instance.SetTransitionMode(ScreenTransitionManager.TransitionMode.WipeLeft);
        SceneTransition.Instance.TransitionToScene(sceneName);
    }
    public void SetDead()
    {
        isDead = true;
        StartCoroutine(ShowDeathScreen());
    }

    private IEnumerator ShowDeathScreen()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        TimeManager.Instance.PauseGame();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GetComponent<Animator>().SetTrigger("Open");
        Time.timeScale = 0f;
        yield return null;
    }

    public void PressPause()
    {
        if (!isPaused)
        {
            isPaused = true;
            PauseGame();
        }
        else
        {
            if (mainMenuHandle && mainMenuHandle.IsAnimationName("Popup"))
            {
                isPaused = false;
                ResumeGame();
            }
            else if (mainMenuHandle && mainMenuHandle.IsAnimationName("HowToPlayIdle"))
            {
                mainMenuHandle.CloseHowToPlayButton();
            }
            else if (mainMenuHandle && mainMenuHandle.IsAnimationName("SettingsPopUp"))
            {
                mainMenuHandle.CloseSettingsButton();
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isDead == false)
        {
            PressPause();
        }
        if (playerEntity)
        {
            if (playerEntity.GetCurrentHealth() <= 0 && isDead == false)
            {
                SetDead();
            }
        }

    }

    void PauseGame()
    {
        TimeManager.Instance.PauseGame();
        pauseUI.GetComponent<Animator>().SetTrigger("Open");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
    }

    void ResumeGame()
    {
        TimeManager.Instance.ResumeGame();
        pauseUI.GetComponent<Animator>().SetTrigger("Close");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ResumeGameAnimationComplete()
    {
        TimeManager.Instance.ResumeGame();
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
