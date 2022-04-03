using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    [SerializeField] private ToDoContentScriptableObject[] days;
    [SerializeField] private Image streakImage;

    public SaveObject so;

    private void Start()
    {
        LoadData();
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

        Debug.Log("Loaded Data");
    }

    #endregion
}
