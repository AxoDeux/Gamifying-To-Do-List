using UnityEngine;

public class SaveTest : MonoBehaviour
{
    public SaveObject so;

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveManager.Save(so);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            so = SaveManager.Load(so.name);
        }
    }
}
