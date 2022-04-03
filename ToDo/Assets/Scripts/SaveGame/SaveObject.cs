[System.Serializable]
public class SaveObject 
{
    public string[] tasks = new string[5];
    public bool[] status = new bool[5];
    public int newActiveTextObj;
    public string name;         //will be the name of the file


    public void SaveData(ToDoContentScriptableObject data)
    {
        for (int i = 0; i < 5; i++)
        {
            tasks[i] = data.tasks[i];
            status[i] = data.taskStatus[i];
        }
        name = data.name;
        newActiveTextObj = data.newTextObjectNumber;
    }

    public void LoadData(ToDoContentScriptableObject data)
    {
        for (int i = 0; i < 5; i++)
        {
            data.tasks[i] = tasks[i];
            data.taskStatus[i] = status[i];
        }
        data.name = name;
        data.newTextObjectNumber = newActiveTextObj;
    }
}

