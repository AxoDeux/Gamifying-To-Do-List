[System.Serializable]
public class SaveGameObject
{
    public string dayOfWeek;
    public string name = "SaveGameData";

    private int lastDayOfPlaying;
    public int LastDayOfPlaying {
        get{ return lastDayOfPlaying; }
    }

    private int streakLength;
    public int StreakLength{
        get { return streakLength; }
    }

    private int totalGoals;
    public int TotalGoals {
        get { return totalGoals; }
        set { totalGoals = value; }
    }

    public string[] goalNames = new string[1];

    public void SaveData(int lastDayNum, int streakScore, string dayName)
    {
        lastDayOfPlaying = lastDayNum;
        streakLength = streakScore;
    }

    public void SaveGoalNames(Goal[] goals) {
        totalGoals = goals.Length;
        goalNames = new string[totalGoals];

        for(int i = 0; i < totalGoals; i++) {
            goalNames[i] = goals[i].goalText.text;
        }
    }
}
