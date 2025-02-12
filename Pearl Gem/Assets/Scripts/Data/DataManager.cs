using System;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public const string CoinKey = "coins";
    public const string LevelKey = "level";
    public const string RainbowBallsKey = "rainbowBalls";
    public const string LevelPearlScoreKey = "levelPearlScore";
    public const string TotalPearlScoreKey = "totalPearlScore";
    public const string GemsKey = "gems";
    public const string SoundOnKey = "soundOn";
    public const string VibrationOnKey = "vibrationOn";
    
    private string _filePath => Application.persistentDataPath + "/gamedata.json";
    private GameData _gameData;

    private void Awake()
    {
        if (Services.DataManager == null)
        {
            Services.Register(this);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        LoadData();
    }

    public void SaveData()
    {
        var json = JsonUtility.ToJson(_gameData, true);
        File.WriteAllText(_filePath, json);
    }

    public void LoadData()
    {
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            _gameData = JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            CreateNewSave();
            SaveData();
        }
    }
    
    private void CreateNewSave()
    {
        _gameData = new GameData
        {
            coins = 0,
            level = 1,
            rainbowBalls = 0,
            levelPearlScore = 0,
            totalPearlScore = 0,
            gems = 0,
            soundOn = true,
            vibrationOn = true
        };

        SaveData();
    }

    public void SaveField<T>(string fieldName, T value)
    {
        LoadData();

        var field = typeof(GameData).GetField(fieldName);
        if (field != null)
        {
            field.SetValue(_gameData, value);
            SaveData();
        }
        else
        {
            Debug.LogError($"Field '{fieldName}' not found");
        }
    }

    public T LoadField<T>(string fieldName)
    {
        LoadData();

        var field = typeof(GameData).GetField(fieldName);
        if (field != null)
        {
            return (T)field.GetValue(_gameData);
        }

        Debug.LogError($"Field '{fieldName}' not found");
        return default;
    }
}
