using TMPro;
using UnityEditor;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    #region MainMenu
    public void OnStartButton()
    {
        GameManager.Instance.ResetRoundScore();
        SceneLoader.Instance.LoadScene(MyScenes.MainGame);
    }
    public void OnSettingsButton()
    {
        SceneLoader.Instance.LoadScene(MyScenes.MainSettings);
    }
    public void OnScoreBoardButton()
    {
        SceneLoader.Instance.LoadScene(MyScenes.MainScoreBoard);
    }
    public void OnQuitButton()
    {
#if UNITY_EDITOR

        GameManager.Instance.Save();
        EditorApplication.isPlaying = false;
#endif

        GameManager.Instance.Save();
        Application.Quit();
    }
    private void Start()
    {
        if (nameInputField != null) nameInputField.text = GameManager.Instance.saveFile.playerName;
        
    }
    public void OnNewName()
    {
        GameManager.Instance.saveFile.playerName = nameInputField.text;
        GameManager.Instance.Save();
    }
    #endregion

    #region General
    public void OnBackButton()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene(MyScenes.MainMenu);
        GameManager.Instance.Save();
    }
    #endregion

}
