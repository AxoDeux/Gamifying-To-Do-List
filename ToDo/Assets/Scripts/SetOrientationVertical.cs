using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOrientationVertical : MonoBehaviour
{
    private void Awake() {
        Screen.orientation = ScreenOrientation.Portrait;
    }
}
