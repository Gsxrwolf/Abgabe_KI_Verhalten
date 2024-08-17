using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoardManager : MonoBehaviour
{

    private List<Round> curScoreBuffer = new List<Round>();
    private List<Round> lastScoreboard = new List<Round>();
    private List<Round> updatedScoreboard = new List<Round>();

    [SerializeField] private GameObject[] contents;

    private List<TextMeshProUGUI> scoreTextFields = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> nameTextFields = new List<TextMeshProUGUI>();


    private void Start()
    {
        for (int i = 0; i < contents.Length; i++)
        {
            nameTextFields.Add(contents[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>());
            scoreTextFields.Add(contents[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>());
        }

        curScoreBuffer = GameManager.Instance.saveFile.scoreBuffer;
        lastScoreboard = GameManager.Instance.saveFile.scoreboard;
        foreach (Round processedScore in curScoreBuffer)
        {
            if (CheckIfScoreboardHasEmptySlots())
            {
                SortScoreInScoreBoard(processedScore);
            }
            else
            {
                if (CheckIfProcessedScoreIsHigherThanLowestScore(processedScore))
                {
                    DeleteLowestScore();
                    SortScoreInScoreBoard(processedScore);
                }
            }

        }
        updatedScoreboard = lastScoreboard;
        SendUpdatedScoreBoardToGameManagerAndResetCurScore();
        PrintUpdatedScoreBoardToText();
    }

    private bool CheckIfProcessedScoreIsHigherThanLowestScore(Round _processedScore)
    {
        lastScoreboard.Sort((round1, round2) => round2.playerScore.CompareTo(round1.playerScore));
        if (_processedScore.playerScore > lastScoreboard[0].playerScore)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SendUpdatedScoreBoardToGameManagerAndResetCurScore()
    {
        GameManager.Instance.saveFile.scoreBuffer.Clear();
        GameManager.Instance.saveFile.scoreboard = updatedScoreboard;
    }

    private void PrintUpdatedScoreBoardToText()
    {
        updatedScoreboard.Sort((round1, round2) => round2.playerScore.CompareTo(round1.playerScore));
        updatedScoreboard.Reverse();
        foreach (GameObject content in contents)
        {
            content.SetActive(false);
        }
        for (int i = 0; i < updatedScoreboard.Count; i++)
        {
            contents[i].SetActive(true);
            nameTextFields[i].text = updatedScoreboard[i].playerName;
            scoreTextFields[i].text = updatedScoreboard[i].playerScore.ToString();
        }
    }


    private void DeleteLowestScore()
    {
        lastScoreboard.Sort();
        lastScoreboard.RemoveAt(0);
    }

    private void SortScoreInScoreBoard(Round _processedScore)
    {
        lastScoreboard.Add(_processedScore);
        lastScoreboard.Sort((round1, round2) => round2.playerScore.CompareTo(round1.playerScore));
    }

    public bool CheckIfScoreboardHasEmptySlots()
    {
        if (lastScoreboard.Count < 10)
        {
            return true;
        }
        if (lastScoreboard.Count >= 10)
        {
            return false;
        }
        return true;
    }
}
