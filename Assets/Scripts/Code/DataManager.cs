using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DataManager : GenericSingletonClass<DataManager>
{
     private int _overallScore;
     private int _currentScore;
     private string _reason;

    private const string OverAllScore = "OverAllScore";
    private const string HalfPlayedData = "LevelData";

    public int OverallScore
    {
        get => _overallScore;
        private set
        {
            if (_overallScore != value)
            {
                _overallScore = value;
            }
        }
    }

    public int CurrentScore
    {
        get => _currentScore;
         private set
        {
            if (_currentScore != value)
            {
                _currentScore = value;
            }
        }
    }

    public string Reason
    {
        get => _reason;
        private set
        {
            if (_reason != value)
            {
                _reason = value;
            }
        }
    }

    public void AddToOverAllScore(int score)
    {
        OverallScore += score;
    }

    public void SetLastPlayedScore(int score)
    {
        CurrentScore = score;
    }

    public void SetReason(string text)
    {
        Reason = text;
    }

    public void SetLevelData(LevelData levelData )
    {
        PlayerPrefs.SetString(HalfPlayedData, JsonConvert.SerializeObject( levelData));
        Debug.Log(JsonConvert.SerializeObject(levelData));
    }

    public LevelData GetLevelData()
    {
        
        string levelString = PlayerPrefs.GetString(HalfPlayedData, string.Empty);
        if (levelString != string.Empty)
        {
            LevelData data = new LevelData();
            data = JsonConvert.DeserializeObject<LevelData>(levelString);
            return data;
        }
        else 
        {
            return null;
        }
       
    }

    public int GetOverAllScore()
    {
        return OverallScore;
    }

    public int GetLastPlayedScore()
    {
        return CurrentScore;
    }
    public string GetReason()
    {
        return Reason;
    }

    public void ResetCurrentScore()
    {
        CurrentScore = 0;
    }

    private void OnEnable()
    {
       OverallScore = PlayerPrefs.GetInt(OverAllScore, 0);
  
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt(OverAllScore, _overallScore);
      
    }
}
