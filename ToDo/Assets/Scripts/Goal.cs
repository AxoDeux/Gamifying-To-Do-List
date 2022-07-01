using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Goal : MonoBehaviour
{
    public GoalScriptableObject goalSO = null;
    public GoalsDataManager goalsDataManager;    //subGoalsText references

    public Button goalButton = null;
    public TMP_Text goalText = null;

    public void OnClickGoal() {
        for(int i = 0; i < goalSO.subGoalNames.Length; i++) {
            goalsDataManager.subGoalsNames[i].text = goalSO.subGoalNames[i];
            for(int j = 0; j < goalSO.subGoalLevel[i]; j++) {
                goalsDataManager.subGoalButtonsParent[i].transform.GetChild(j).GetComponent<Image>().color = Color.green;
            }
        }
    }
}
