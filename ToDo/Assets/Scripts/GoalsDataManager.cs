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
    [SerializeField] private GameObject goalInputField = null;
    [SerializeField] private GameObject subGoalInputField = null;

    [SerializeField] private GoalScriptableObject[] goals;
    [SerializeField] private List<Goal> goalsList = new List<Goal>();
    [SerializeField] private List<SubGoal> subGoalsList = new List<SubGoal>();

    public SaveGameObject saveGameObject;
    public SaveGoalsObject saveGoalsObject;

    private int totalGoals;
    private string[] goalNames;
    private List<string> goalNamesList = new List<string>();
    private List<Goal> deletedGoals = new List<Goal>();


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
        SetSubGoalData();
        SaveData();
        DeleteGoals();
        SceneManager.LoadScene(0);
    }

    private void LoadData() {
        //Get data from saveGameObject (total goal, goalNames)
        saveGameObject = SaveManager.LoadGameData("SaveGameData");
        if(saveGameObject != null) {
            totalGoals = saveGameObject.TotalGoals;
            goalNames = new string[totalGoals];
            for(int i = 0; i < totalGoals; i++) {

                goalNames[i] = saveGameObject.goalNames[i];
                goalNamesList.Add(saveGameObject.goalNames[i]);
            }
        }

        //get data from saveGoalObject and add the data into a scriptable object
        goals = new GoalScriptableObject[totalGoals];
        for(int i = 0; i< totalGoals; i++) {
            //create GoalScriptableObject
            string fileName = string.Concat(goalNames[i].Where(c => !Char.IsWhiteSpace(c)));        //remove spaces from goalName cuz file cannot have spaces.
            saveGoalsObject = SaveManager.LoadGoalsData(fileName);     
            goals[i] = (GoalScriptableObject)ScriptableObject.CreateInstance(typeof(GoalScriptableObject));
            saveGoalsObject.LoadData(goals[i]);
        }
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
        goalInputField.SetActive(true);
    }

    public void AddSubGoal() {
        subGoalInputField.SetActive(true);
    }

    public void SetGoalName(TMP_InputField value) {
        GameObject goalObject = Instantiate(goalPrefab, goalsContentParent);
        Goal goal = goalObject.GetComponent<Goal>();
        goal.goalText.text = value.text;

        value.text = null;
        goalInputField.SetActive(false);

        GoalScriptableObject goalTempSO = (GoalScriptableObject)ScriptableObject.CreateInstance(typeof(GoalScriptableObject));
        goal.goalSO = goalTempSO;
        goal.goalSO.name = goal.goalText.text;
        goal.goalSO.goalName = goal.goalText.text;

        goal.goalsDataManager = this;
        goalsList.Add(goal);

        SaveData();
    }

    public void SetSubGoalName(TMP_InputField value) {
        //Get the value of an empty subgoal in order.
        //Add the text to that subgoal.
    }

    private void SaveData() {
        foreach(Goal goal in goalsList) {
            goal.goalSO.Save();
        }
        saveGameObject.SaveGoalNames(goalsList.ToArray());
        SaveManager.SaveGameData(saveGameObject);
    }

    private void HandleGoalSelectEvent(Goal goal) {
        SetSubGoalData();
        SaveData();

        selectedGoal = goal;

        //Set the ScriptableObject data in the subgoals
        int i = 0;
        foreach(SubGoal subGoal in subGoalsList) {
            subGoal.SetData(goal.goalSO.subGoalNames[i], goal.goalSO.subGoalLevel[i]);
            i++;
        }
    }

    //Set user added subgoal data in GoalScriptableObject
    private void SetSubGoalData() {
        if(!selectedGoal) { return; }
        int i = 0;
        foreach(SubGoal subGoal in subGoalsList) {
            selectedGoal.goalSO.subGoalNames[i] = subGoal.subGoalName.text;
            selectedGoal.goalSO.subGoalLevel[i] = subGoal.subGoalLevel;
            i++;
        }
    }

    public void DeleteOrUndo(TMP_Text buttonText) {
        if(selectedGoal == null) { return; }

        if(buttonText.text  == "Delete") {
            buttonText.text = "Undo";
            selectedGoal.gameObject.SetActive(false);
            deletedGoals.Add(selectedGoal);
        }

        if(buttonText.text == "Undo") {
            buttonText.text = "Delete";
            selectedGoal.gameObject.SetActive(true);
            deletedGoals.Remove(selectedGoal);
        }
    }

    //TO DO: Delete respective file from the save folder
    private void DeleteGoals() {
        foreach(Goal goal in deletedGoals) {
            goalsList.Remove(goal);
            Destroy(goal.gameObject);
        }
    }
}
