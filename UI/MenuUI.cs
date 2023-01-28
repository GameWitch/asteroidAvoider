using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    TMP_Dropdown dropdown;
    ScoreKeeper scoreKeeper;

    List<TMP_Dropdown.OptionData> levelOptions = new List<TMP_Dropdown.OptionData>();
    int highestLevel;
    private void Awake()
    {
        scoreKeeper = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreKeeper>();

        highestLevel = PlayerPrefs.GetInt("HighestLevel", 1);

        dropdown = GetComponentInChildren<TMP_Dropdown>();
        dropdown.ClearOptions();            

        for (int i = 1; i < highestLevel+1; i ++)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = "Level " + i.ToString();
            levelOptions.Add(optionData);            
        }

        dropdown.AddOptions(levelOptions);

    }

    public void LevelToLoad()
    {
        scoreKeeper.SetLevel(dropdown.value + 1);
    }

    public void OpenSoundSettings()
    {
        SceneManager.LoadScene(2);
    }



}
