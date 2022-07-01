using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    [SerializeField] private ToDoContentScriptableObject[] days;
    [SerializeField] private Image streakImage;
    [SerializeField] private TMP_Text streakScoreString;
    [SerializeField] private TMP_Text dayString;


    private int lastDayOfPlaying = 0;
    private int currentDayNum = 0;
    private string currentDay = "";
    private int streakScore = 0;

    public SaveObject so;
    public SaveGameObject svgo;

    private void Start()
    {
        currentDayNum = System.DateTime.Now.DayOfYear;
        dayString.text = System.DateTime.Now.DayOfWeek.ToString();

        LoadData();

        SaveGameObject sgo = SaveManager.LoadGameData("SaveGameData");
        svgo.SaveData(lastDayOfPlaying, streakScore, currentDay);
        svgo.TotalGoals = sgo.TotalGoals;       //equating the svgo values to saved values to avoid loss of data.
        svgo.goalNames = sgo.goalNames;
        SaveManager.SaveGameData(svgo);
        Debug.Log(svgo.TotalGoals);
    }

    public void OnClickNewWeek()
    {
        for(int i = 0; i < 7; i++)
        {
            for (int j = 0; j<5; j++)
            {
                days[i].tasks[j] = null;
                days[i].taskStatus[j] = false;
            }

            days[i].newTextObjectNumber = 0;
            days[i].OnClickSave();                  //save nulling data
        }
    }


    #region LoadData
    private void LoadData()     //Load the data into the respective scriptable objects
    {
        for (int i = 0; i < 8; i++)
        {
            SaveObject so = SaveManager.Load(days[i].name);
            if (so != null)
            {
                so.LoadData(days[i]);
            }
        }

        SaveGameObject sgo = SaveManager.LoadGameData("SaveGameData");
        if(sgo != null)
        {
            lastDayOfPlaying = sgo.LastDayOfPlaying;
            streakScore = sgo.StreakLength;
            CheckStreak(sgo);
        }        

        Debug.Log("DataManager: Loaded Data");
    }

    #endregion

    private void CheckStreak(SaveGameObject sgo)
    {
        if (currentDayNum - 1 == lastDayOfPlaying)
        {
            streakScore++;
        }
        else if (currentDayNum != lastDayOfPlaying && currentDayNum - 1 == lastDayOfPlaying)        //shouldnt be last day shouldnt be current or prev day
        {
            streakScore = 0;
        }
        streakScoreString.text = streakScore.ToString();

    }

}
