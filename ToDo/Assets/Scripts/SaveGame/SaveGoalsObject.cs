[System.Serializable]
public class SaveGoalsObject {
    public string goalName;             //also the name of saveGoalObject
    public string[] subGoalNames = new string[5];
    public int[] subGoalDoneLevel = new int[5];
    public int nextSubGoalNum;

    public void SaveData(GoalScriptableObject data) {
        goalName = data.goalName;
        nextSubGoalNum = data.newSubGoalNum;
        for(int i =0; i<data.subGoalNames.Length; i++) {
            subGoalNames[i] = data.subGoalNames[i];
            subGoalDoneLevel[i] = data.subGoalLevel[i];
        }
    }

    //load data into scriptable object
    public void LoadData(GoalScriptableObject data) {
        data.goalName = goalName;
        data.newSubGoalNum = nextSubGoalNum;
        for(int i = 0; i < data.subGoalNames.Length; i++) {
            data.subGoalNames[i] = subGoalNames[i];
            data.subGoalLevel[i] = subGoalDoneLevel[i];
        }
    }
}
