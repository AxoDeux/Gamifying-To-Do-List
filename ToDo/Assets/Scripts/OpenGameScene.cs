using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenGameScene : MonoBehaviour
{
    public void OpenScene()
    {
        SceneManager.LoadScene(1);
    }
}
