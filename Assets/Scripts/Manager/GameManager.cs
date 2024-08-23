using System;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public SaveFile saveFile;
    public static event Action<float> DoUIUpdate;

    [HideInInspector] public bool lockScore = false;

    [SerializeField] private int pointsForSheepSpawn = 100;
    [SerializeField] private int pointsForWolfSpawn = 200;
    [SerializeField] private int pointsForWolfEscape = 500;

    private void Start()
    {
        Load();
        LoadSavedSettings();
    }
    private void LoadSavedSettings()
    {
        AudioManager.Instance.SetVolume("Master", saveFile.volumeMaster);
        AudioManager.Instance.SetVolume("Music", saveFile.volumeMusic);
    }

    public void Save()
    {
        var filePath = Application.persistentDataPath + "/VCG/savefiles.json";
        var directoryPath = Path.GetDirectoryName(filePath);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var data = JsonUtility.ToJson(saveFile);

        using (StreamWriter writer = File.CreateText(filePath))
        {
            writer.Write(data);
        }
    }

    public void Load()
    {
        var filePath = Application.persistentDataPath + "/VCG/savefiles.json";
        var directoryPath = Path.GetDirectoryName(filePath);

        if (Directory.Exists(directoryPath))
        {
            using (StreamReader reader = File.OpenText(filePath))
            {
                var data = reader.ReadToEnd();
                JsonUtility.FromJsonOverwrite(data, saveFile);
            }
        }
    }

    public void ResetRoundScore()
    {
        saveFile.playerScore = 0;
    }
    public void SaveRoundScore()
    {
        if (saveFile.playerScore != 0)
        {
            saveFile.scoreBufferName.Add(saveFile.playerName);
            saveFile.scoreBufferValue.Add(saveFile.playerScore);
            ResetRoundScore();
            lockScore = false;
            Save();
        }
    }
    private void LockScore()
    {
        lockScore = true;
    }
    public void AddPointsForSheepSpawn()
    {
        Debug.Log("SheepSpawn");
        saveFile.playerScore += pointsForSheepSpawn;
        DoUIUpdate?.Invoke(saveFile.playerScore);
    }
    public void AddPointsForWolfSpawn()
    {
        Debug.Log("WolfSpawn");
        saveFile.playerScore += pointsForWolfSpawn;
        DoUIUpdate?.Invoke(saveFile.playerScore);
    }
    public void AddPointsForWolfEscape()
    {
        Debug.Log("WolfEscape");
        saveFile.playerScore += pointsForWolfEscape;
        DoUIUpdate?.Invoke(saveFile.playerScore);
    }
}
