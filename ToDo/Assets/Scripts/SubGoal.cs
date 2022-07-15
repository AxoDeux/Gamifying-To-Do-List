using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SubGoal : MonoBehaviour
{
    public TMP_Text subGoalName;
    public int subGoalLevel;

    public List<Button> subGoalButtons = new List<Button>();
    private List<bool> buttonStatus;

    private Dictionary<Button, bool> buttonToStatusMap;
    private Dictionary<Button, int> buttonToIndexMap;
    private Dictionary<int, Button> indexToButtonMap;

    public GoalScriptableObject goalScriptableObject = null;

    private void Awake() {
        buttonStatus = new List<bool>();
        buttonToStatusMap = new Dictionary<Button, bool>();
        buttonToIndexMap = new Dictionary<Button, int>();
        indexToButtonMap = new Dictionary<int, Button>();
        int index = 0;

        foreach(Button button in subGoalButtons) {
            bool status = false;
            button.image.color = Color.black;

            buttonStatus.Add(status);
            buttonToStatusMap.Add(button, status);
            buttonToIndexMap.Add(button, index);
            indexToButtonMap.Add(index, button);
            index++;
        }
    }


    public void OnClickButton(Button button) {
        //Assign Colour and Status
        buttonToStatusMap[button] = !buttonToStatusMap[button];

        //if changing to true check prevButtonStatus, if changing to false check nextButtonsStatus
        if(buttonToStatusMap[button]) {
            if(CheckPrevButtonsStatus(buttonToIndexMap[button])) {
                button.image.color = Color.green;
                subGoalLevel++;
            } else {
                buttonToStatusMap[button] = !buttonToStatusMap[button];
            }
        } else {
            if(CheckNextButtonsStatus(buttonToIndexMap[button])) {
                button.image.color = Color.black;
                subGoalLevel--;
            } else {
                buttonToStatusMap[button] = !buttonToStatusMap[button];
            }
        }
    }

    //Check if the previous buttons are all green
    private bool CheckPrevButtonsStatus(int index) { 
        if(index == 0) { return true; }
        Button button;
        for(int i = 0; i < index; i++) {
            button = indexToButtonMap[i];
            if(!buttonToStatusMap[button]) { return false; }
        }
        return true;
    }

    //Check if the next buttons are all black
    private bool CheckNextButtonsStatus(int index) {
        if(index == 4) { return true; }
        Button button;
        for(int i = 4; i > index; i--) {
            button = indexToButtonMap[i];
            if(buttonToStatusMap[button]) { return false; }
        }
        return true;
    }

    public void SetData(string name, int subgoalLevel) {
        subGoalName.text = name;
        subGoalLevel = subgoalLevel;
        //take the saved staus array
        //replace the buttonstatus, colour as per the highest true status index of the subGoal

        foreach(Button button in subGoalButtons) {
            if(buttonToIndexMap[button] <= subgoalLevel) {
                buttonToStatusMap[button] = true;
                button.image.color = Color.green;
            } else {
                buttonToStatusMap[button] = false;
                button.image.color = Color.black;
            }
        }
    }

}
