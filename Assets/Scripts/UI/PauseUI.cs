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
            paused = !paused;
        }
        if (paused)
        {
            pauseUIToggel.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else
        {
            pauseUIToggel.SetActive(false);
            Time.timeScale = 1.0f;
        }
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
        paused = false;
    }
    public void OnMenuButton()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.ResetRoundScore();
        SceneLoader.Instance.LoadScene(MyScenes.MainMenu);
    }
}
