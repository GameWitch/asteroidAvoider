using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{

    int level = 1;
    float time = 0f;
    bool timerRunning = false;


    void Awake()
    {
        SingletonJawn();
    }

    void Update()
    {
        if (timerRunning)
        {
            time += Time.deltaTime;
        }
    }


    void SingletonJawn()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Score");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetLevel(int levelToBe)
    {
        level = levelToBe;
    }

    public int GetLevel()
    {
        return level;
    }

    public void NextLevel()
    {
        level++;

        RestartLevel();
    }

    public void RestartLevel()
    {
        time = 0f;      
    }

    public string GetTime()
    {
        float minutes = time / 60;
        float seconds = time % 60;
        string timeString = (int)minutes + ":" + seconds.ToString("00.00");
        return timeString;
    }
    public bool IsTimerRunning()
    {
        return timerRunning;
    }
    public void StopTimer()
    {
        PlayerPrefs.SetFloat("ThisTime", time);
        timerRunning = false;
    }

    public void StartTimer()
    {
        timerRunning = true;
    }

    public void SaveHighestLevelOnLose()
    {
        int highestLevel = PlayerPrefs.GetInt("HighestLevel", 0);
        if (level > highestLevel)
        {
            PlayerPrefs.SetInt("HighestLevel", level);
        }
    }

    public void SaveHighScores()
    {
        int highestLevel = PlayerPrefs.GetInt("HighestLevel", 0);
        if (level > highestLevel)
        {
            PlayerPrefs.SetInt("HighestLevel", level);
        }

        string asteroidsBlastedStringRef = "level" + level + "asteroidsBlasted";
        int levelBlastedAsteroids = PlayerPrefs.GetInt(asteroidsBlastedStringRef, 0);

        int asteroidsBlasted = PlayerPrefs.GetInt("AsteroidsBlasted", 0);
        if (levelBlastedAsteroids < asteroidsBlasted)
        {
            PlayerPrefs.SetInt(asteroidsBlastedStringRef, asteroidsBlasted);
        }

        string levelTimeStringRef = "level" + level + "BestTime";
        float levelBestTime = PlayerPrefs.GetFloat(levelTimeStringRef, 0);
        if (time < levelBestTime || levelBestTime == 0)
        {
            PlayerPrefs.SetFloat(levelTimeStringRef, time);
        }
        
    }

    // public for menu button
    public void ResetGame()
    {
        level = 1;
        RestartLevel();
    }
}
