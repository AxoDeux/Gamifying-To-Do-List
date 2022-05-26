using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestTime : MonoBehaviour
{
    [SerializeField] private TMP_Text dateText = null;

    private void Start()
    {
        Debug.Log("The Date and Time is" + System.DateTime.Now.Day);
        Debug.Log("The Days of year" + System.DateTime.Now.DayOfYear);

        dateText.text = $"Date: {System.DateTime.Now}\n Day: {System.DateTime.Now.DayOfWeek}";

    }
}
