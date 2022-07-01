using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tasks")]
public class ToDoContentScriptableObject : ScriptableObject
{
    public int totalTasks = 0;
    public string[] tasks;
    public bool[] taskStatus;

    public int newTextObjectNumber = 0;

    public SaveObject so;

    private void Awake()
    {
        tasks = new string[5];
        taskStatus = new bool[5];
    }

    public void OnAddTask(string task)
    {
        if(newTextObjectNumber >= 5) { return; }
        //Add the task to an empty string
        tasks[newTextObjectNumber] = task;
        newTextObjectNumber++;
    }

    public void RearrangeTasks()                //Rearrange required when task is removed
    {
        for(int i = 0; i < 5; i++)
        {
            if (tasks[i] != null) { continue; }

            for(int j = i; j<5; j++)
            {
                if(j == 4) 
                {
                    tasks[j] = null;
                    return;
                }
                tasks[j] = tasks[j + 1];
            }
        }
    }

    public void OnClickSave()
    {
        SaveTasks();
    }

    private void SaveTasks()
    {
        so.SaveData(this);
        SaveManager.Save(so);
    }
}
