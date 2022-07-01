using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class GoalsDataManager : MonoBehaviour
{
    private const string GOAL_NAME_TEXT = "t_GoalName";
    private const string GOAL_BUTTON = "b_GoalButton";

    [SerializeField] private GameObject goalPrefab = null;
    [SerializeField] private Transform goalsContentParent = null;
    [SerializeField] private GameObject inputField = null;

    [SerializeField] private GoalScriptableObject[] goals;
    [SerializeField] private List<Goal> goalsList = new List<Goal>();

    public SaveGameObject saveGameObject;
    public SaveGoalsObject saveGoalsObject;

    private int totalGoals;
    private string[] goalNames;
    private List<string> goalNamesList = new List<string>();
    public TMP_InputField[] subGoalsNames = new TMP_InputField[5];
    public GameObject[] subGoalButtonsParent = new GameObject[5];

    private Goal selectedGoal;

    private void Awake() {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    private void Start() {
        SaveGameObject sgo = SaveManager.LoadGameData("SaveGameData");
        saveGameObject.SaveData(sgo.LastDayOfPlaying, sgo.StreakLength, sgo.dayOfWeek);         //To avoid Loss of data

        LoadData();
        CreateGoalPrefabs();
    }

    private void OnEnable() {
        Goal.OnSelectGoalEvent += HandleGoalSelectEvent;
    }

    private void OnDisable() {
        Goal.OnSelectGoalEvent -= HandleGoalSelectEvent;
    }

    public void OpenMainScene() {
        SaveData();
        SceneManager.LoadScene(0);
    }

    private void LoadData() {
        saveGameObject = SaveManager.LoadGameData("SaveGameData");
        if(saveGameObject != null) {
            totalGoals = saveGameObject.TotalGoals;
            goalNames = new string[totalGoals];
            for(int i = 0; i < totalGoals; i++) {
                goalNames[i] = saveGameObject.goalNames[i];
                goalNamesList.Add(saveGameObject.goalNames[i]);
            }
        }

        goals = new GoalScriptableObject[totalGoals];
        for(int i = 0; i< totalGoals; i++) {
            //create GoalScriptableObject
            string fileName = string.Concat(goalNames[i].Where(c => !Char.IsWhiteSpace(c)));        //remove spaces from goalName cuz file cannot have spaces.
            saveGoalsObject = SaveManager.LoadGoalsData(fileName);     
            goals[i] = (GoalScriptableObject)ScriptableObject.CreateInstance(typeof(GoalScriptableObject));
            saveGoalsObject.LoadData(goals[i]);
        }

        Debug.Log("GoalsDataManager: Data loaded.");
    }

    private void CreateGoalPrefabs() {
        for(int i = 0; i < totalGoals; i++) {
            GameObject goalObject = Instantiate(goalPrefab, goalsContentParent);
            Goal goal = goalObject.GetComponent<Goal>();
            goal.goalSO = goals[i];
            goal.goalsDataManager = this;
            goal.goalText.text = goals[i].goalName;
            goalsList.Add(goal);
        }
    }


    public void AddGoal() {
        inputField.SetActive(true);
    }

    public void SetGoalName(TMP_InputField value) {
        GameObject goalObject = Instantiate(goalPrefab, goalsContentParent);
        Goal goal = goalObject.GetComponent<Goal>();
        goal.goalText.text = value.text;

        value.text = null;
        inputField.SetActive(false);

        GoalScriptableObject goalTempSO = (GoalScriptableObject)ScriptableObject.CreateInstance(typeof(GoalScriptableObject));
        goal.goalSO = goalTempSO;
        goal.goalSO.name = goal.goalText.text;
        goal.goalSO.goalName = goal.goalText.text;

        goal.goalsDataManager = this;
        goalsList.Add(goal);

        SaveData();
    }

    private void SaveData() {
        foreach(Goal goal in goalsList) {
            goal.goalSO.Save();
        }
        saveGameObject.SaveGoalNames(goalsList.ToArray());
        SaveManager.SaveGameData(saveGameObject);

        Debug.Log(saveGameObject.TotalGoals);
    }

    private void HandleGoalSelectEvent(Goal goal) {
        selectedGoal = goal;
    }

    public void DeleteSelectedGoal() {
        goalsList.Remove(selectedGoal);
        Destroy(selectedGoal.gameObject);
        selectedGoal = null;
    }
}
