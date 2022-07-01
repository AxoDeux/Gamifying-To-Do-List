using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToDoContent : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _tasks = null;
    [SerializeField] private TMP_Text _dayName = null;

    private ToDoContentScriptableObject thisDay;

    public void AddPreviousData(ToDoContentScriptableObject data)
    {
        thisDay = data;
        Debug.Log("Setting previous Values");
        for(int i = 0; i<5;  i++)
        {
            _tasks[i].text = data.tasks[i];
            SetTaskStatus(i);
        }
        _dayName.text = thisDay.name;
    }


    public void OnClickBack()
    {
        thisDay.OnClickSave();
        this.gameObject.SetActive(false);
    }

    public void OnEditText(TMP_InputField inputField)
    {
        if(string.IsNullOrWhiteSpace(inputField.text)){
            inputField.text = null;
            return;
        }
        thisDay.OnAddTask(inputField.text);             //set task in the scriptable object itself
        AddTask(inputField.text);
        inputField.text = null;
    }

    public void OnRemove(int textNumber)            //remove task
    {
        if(thisDay.tasks[textNumber] == null) { return; }
        Debug.Log("Removed Task");
        thisDay.tasks[textNumber] = null;
        thisDay.RearrangeTasks();
        AddPreviousData(thisDay);

        thisDay.newTextObjectNumber--;
        if(thisDay.newTextObjectNumber < 0) {
            thisDay.newTextObjectNumber = 0;
        }
    }

    public void TaskComplete(int textNumber)
    {
        if (!thisDay.taskStatus[textNumber])
        {
            _tasks[textNumber].alpha = 0.5f;
            _tasks[textNumber].fontStyle = FontStyles.Strikethrough;
            thisDay.taskStatus[textNumber] = true;
        }
        else
        {
            _tasks[textNumber].alpha = 1f;
            _tasks[textNumber].fontStyle = FontStyles.Normal;
            thisDay.taskStatus[textNumber] = false;
        }
    }

    private void AddTask(string taskText)
    {
        _tasks[thisDay.newTextObjectNumber -1].text = taskText;     //set text in the canvas
    }

    private void SetTaskStatus(int taskNum)
    {
        if (!thisDay.taskStatus[taskNum])
        {
            _tasks[taskNum].alpha = 1f;
            _tasks[taskNum].fontStyle = FontStyles.Normal;
        }
        else
        {
            _tasks[taskNum].alpha = 0.5f;
            _tasks[taskNum].fontStyle = FontStyles.Strikethrough;
        }
    }

}
