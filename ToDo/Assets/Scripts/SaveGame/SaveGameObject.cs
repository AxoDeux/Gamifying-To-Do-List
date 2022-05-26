[System.Serializable]
public class SaveGameObject
{
    public int lastDayOfPlaying;
    public string dayOfWeek;
    public string name = "SaveGameData";
    public int streakLength;

    public void SaveData(int lastDayNum, int streakScore, string dayName)
    {
        lastDayOfPlaying = lastDayNum;
        streakLength = streakScore;
    }

    public int LoadLastDayOfPlaying()
    {
        return lastDayOfPlaying;
    }

    public int LoadStreakLength()
    {
       return streakLength;
    }
}
