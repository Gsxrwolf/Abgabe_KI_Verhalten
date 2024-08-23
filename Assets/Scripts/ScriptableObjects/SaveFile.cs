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
    [SerializeField] public List<string> scoreboardName = new List<string>();
    [SerializeField] public List<int> scoreboardValue = new List<int>();
    [SerializeField] public List<string> scoreBufferName = new List<string>();
    [SerializeField] public List<int> scoreBufferValue = new List<int>();
}
