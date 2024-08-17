using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundOverUIUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private GameObject backgroundToggel;
    [SerializeField] private GameObject normalScoreField;

    private void Awake()
    {
        PoolSpawner.RoundIsOver += ShowRoundIsOver;
    }

    private void ShowRoundIsOver()
    {
        scoreText.text = "Score: " + GameManager.Instance.saveFile.playerScore;
        backgroundToggel.SetActive(true);
        normalScoreField.SetActive(false);
    }

    public void OnExitToMenu()
    {
        GameManager.Instance.SaveRoundScore();
        SceneLoader.Instance.LoadScene(MyScenes.MainMenu);
    }

    private void OnDestroy()
    {
        PoolSpawner.RoundIsOver -= ShowRoundIsOver;
    }
}
