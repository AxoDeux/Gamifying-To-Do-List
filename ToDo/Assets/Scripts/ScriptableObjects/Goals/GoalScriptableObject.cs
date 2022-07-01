using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Goal")]
public class GoalScriptableObject : ScriptableObject
{
    public string goalName;
    public string[] subGoalNames;
    public int[] subGoalLevel;
    public int newSubGoalNum = 0;

    public SaveGoalsObject svgo = new SaveGoalsObject();

    private void Awake() {
        subGoalNames = new string[5];
        subGoalLevel = new int[5];
    }

    public void AddSubGoal(string subGoal) {
        if(newSubGoalNum >= 5) { return; }
        subGoalNames[newSubGoalNum] = subGoal;
        newSubGoalNum++;
    }

    //Save data into savegoalobject
    public void Save() {
        SaveData();
    }

    private void SaveData() {
        svgo.SaveData(this);
        SaveManager.Save(svgo);
    }
}
