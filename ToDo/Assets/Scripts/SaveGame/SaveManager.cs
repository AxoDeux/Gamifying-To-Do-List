using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager             //static means one reference only
{
    public static string directory = "SaveData";
    //public static string fileName;

    #region SaveObject
    public static void Save(SaveObject so)
    {
        if (!DirectoryExists())                                                             //Check if the directory exists
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + directory);
        }
        BinaryFormatter bf = new BinaryFormatter();
        string fileName = so.name + ".txt";
        FileStream file = File.Create(GetFullPath(fileName));              //creates new file, if it exists, we will overwrite it
        bf.Serialize(file, so);             //pass file and the object we  want to save in it
        file.Close();                       //imp or else the file will be open and will lead to errors
    }

    public static SaveObject Load(string name)      //takes string name and finds a file of the same name
    {
        string fileName = name + ".txt";
        if (SaveExists(fileName))
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(GetFullPath(fileName), FileMode.Open);
                SaveObject so = (SaveObject)bf.Deserialize(file);
                file.Close();

                Debug.Log("Found file");
                return so;
            }
            catch (SerializationException)          //one common reason for serilization exception to be called is when someone tries to manually change the file
            {
                Debug.Log("Failed to load file");       
            }
        }

        return null;
    }

    #endregion

    private static bool SaveExists(string fileName)
    {
        return File.Exists(GetFullPath(fileName));          //"file" is a system.io method
    }

    private static bool DirectoryExists()
    {
        return Directory.Exists(Application.persistentDataPath + "/" + directory);
    }

    private static string GetFullPath(string fileName)
    {
        return Application.persistentDataPath + "/" + directory + "/" + fileName;           //persistentDataPath exists for all platforms
    }
}
