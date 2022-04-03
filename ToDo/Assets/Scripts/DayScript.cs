using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayScript : MonoBehaviour
{
    [SerializeField] private GameObject toDoPrefab = null;
    [SerializeField] private ToDoContentScriptableObject thisDayContent = null;

    private ToDoContent toDoContentScript;
    private void Start()
    {
        toDoContentScript = toDoPrefab.GetComponent<ToDoContent>();
    }
    public void OnClickDay()
    {
        toDoPrefab.SetActive(true);
        toDoContentScript.AddPreviousData(thisDayContent);
    }


}
