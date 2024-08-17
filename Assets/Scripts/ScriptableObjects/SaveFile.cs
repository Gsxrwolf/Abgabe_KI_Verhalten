using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveValues", menuName = "tools/saveValues")]
public class SaveFile : ScriptableObject
{
    [SerializeField] public float volumeMaster;
    [SerializeField] public float volumeMusic;
    [SerializeField] public int qualitySetting;
    [SerializeField] public string playerName = "Player";
    [SerializeField] public int playerScore;
    [SerializeField] public List<Round> scoreboard = new List<Round>();
    [SerializeField] public List<Round> scoreBuffer = new List<Round>();
}
