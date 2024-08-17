using TMPro;
using UnityEngine;

public class UiUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        GameManager.DoUIUpdate += DoUpdate;
    }

    private void DoUpdate(float _newScore)
    {
        scoreText.text = "Score: " + _newScore;
    }

    private void OnDestroy()
    {
        GameManager.DoUIUpdate -= DoUpdate;
    }
}
