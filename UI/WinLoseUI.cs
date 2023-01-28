
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class WinLoseUI : MonoBehaviour
{

    
    [SerializeField] TMP_Text message;
    [SerializeField] TMP_Text buttonText;
    [SerializeField] TMP_Text thisLevel;
    [SerializeField] TMP_Text levelBestTime;
    [SerializeField] TMP_Text mostThrustOnLevel;
    [SerializeField] TMP_Text timeThisRun;
    [SerializeField] TMP_Text distanceTraveled;
    [SerializeField] TMP_Text thrustUsed;
    [SerializeField] TMP_Text mostBlastedAsteroids;
    [SerializeField] TMP_Text asteroidsBlastedThisTime;

    PlayerControls playerControls;
    string level;


    private void Update()
    {

        if (playerControls != null)
        {
            float retryValue = playerControls.UI.Submit.ReadValue<float>();
            if (retryValue > 0)
            {
                ReloadLevel();
            }
            float menuValue = playerControls.UI.Cancel.ReadValue<float>();
            if (menuValue > 0)
            {
                MainMenu();
            }
        }


    }

    private string FormatTime(float time)
    {
        float minutes = time / 60;
        float seconds = time % 60;
        string timeString = (int)minutes + ":" + seconds.ToString("00.00");
        return timeString;
    }

    public  void GetPlayerControls(PlayerControls pc)
    {
        playerControls = pc;
    }

    public void Win()
    {
        message.text = "hell yea";
        buttonText.text = "Next";

        PostScore();
    }

    public void Lose()
    {
        
        message.text = "Damn Your whole shit blew up";
        buttonText.text = "Retry";
        
        PostScore();
    }

    public void GetLevel(int _level)
    {
        level = _level.ToString();
    }
    private void PostScore()
    {
        thisLevel.text = level;

        string levelTimeStringRef = "level" + level + "BestTime";
        float bestTime = PlayerPrefs.GetFloat(levelTimeStringRef);
        levelBestTime.text = FormatTime(bestTime);

        float thisTime = PlayerPrefs.GetFloat("ThisTime");
        timeThisRun.text = FormatTime(thisTime);

        asteroidsBlastedThisTime.text = PlayerPrefs.GetInt("AsteroidsBlasted", 0).ToString();

        string asteroidsBlastedStringRef = "level" + level + "asteroidsBlasted";
        mostBlastedAsteroids.text = PlayerPrefs.GetInt(asteroidsBlastedStringRef).ToString();

    }


    public void ReloadLevel()
    {
        // public so it can be on a UI button

        PlayerPrefs.SetInt("AsteroidsBlasted", 0);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
