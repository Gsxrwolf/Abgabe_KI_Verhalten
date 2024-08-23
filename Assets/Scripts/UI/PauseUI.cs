using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    private bool paused;

    [SerializeField] private GameObject pauseUIToggel;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused) Unpause();
            else Pause();
        }
    }

    private void Unpause()
    {
        paused = false;
        pauseUIToggel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    private void Pause()
    {   
        paused = true;
        pauseUIToggel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void OnQuitButton()
    {
        GameManager.Instance.ResetRoundScore();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }
    public void OnXButton()
    {
        Unpause();
    }
    public void OnMenuButton()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.ResetRoundScore();
        SceneLoader.Instance.LoadScene(MyScenes.MainMenu);
    }
}
