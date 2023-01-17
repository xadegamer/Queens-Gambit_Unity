using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] int maxLevel;
    [SerializeField] int pumpkin = 0;
    [SerializeField] int score;
    [SerializeField] int lastUnlockedLevel;

    [SerializeField] PlayerData data;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Load();

            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public int GetScore() => score;
    public int GetMaxLevel() => maxLevel;
    public int GetLastUnlockedLevel() => lastUnlockedLevel;
    public int GetPumpkins() => pumpkin;

    public void CollectPumpkin(int amount)
    {
        pumpkin += amount;
    }

    public void IncrementScore(int amount)
    {
        score += amount;
        UIManager.Instance.SetScroreText(score);
    }

    public void IncrementLevel()
    {
        lastUnlockedLevel++;
        Save();
    }

    public void Save()
    {
        data.SetPumpkins(pumpkin);
        data.SetLastUnlockedLevel(lastUnlockedLevel);
        data.SetScore(score);
       JsonSaveSystem.Save("PlayerData", data);
    }

    public void Load()
    {
        PlayerData playerData = JsonSaveSystem.Load<PlayerData>("PlayerData");
        if (playerData != null) data = playerData; else data = new PlayerData();

        pumpkin = data.GetPumpkins();
        lastUnlockedLevel = data.GetLastUnlockedLevel();
        score = data.GetScore();
    }
}

[System.Serializable]
public class PlayerData
{
    [SerializeField] int savedPumpkins = 0;
    [SerializeField] int savedScore;
    [SerializeField] int savedLastUnlockedLevel = 1;

    public void SetPumpkins(int amount) => savedPumpkins = amount;
    public int GetPumpkins() => savedPumpkins;
    public int SetLastUnlockedLevel(int level) => savedLastUnlockedLevel = level;
    public int GetLastUnlockedLevel() => savedLastUnlockedLevel;
    public void SetScore(int amount) => savedScore = amount;
    public int GetScore() => savedScore;
}
