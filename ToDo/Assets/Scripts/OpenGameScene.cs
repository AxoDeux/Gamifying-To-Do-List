using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.InputSystem.EnhancedTouch;

public class OpenGameScene : MonoBehaviour
{
    public void OpenScene()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenGoalScene() {
        SceneManager.LoadScene(2);
    }
}
