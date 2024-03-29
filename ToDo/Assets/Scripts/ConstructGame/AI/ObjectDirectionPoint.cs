using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDirectionPoint : MonoBehaviour
{
    private const string REQUIRED_OBJECT = "EnemyRequiredComponents";
    private const int GRID_CENTER_INDEX = 0;
    [SerializeField]
    private Transform gridCenterObject = null;

    private GameObject requiredComponentObject;

    private void Start() {
        requiredComponentObject = GameObject.Find(REQUIRED_OBJECT);
        gridCenterObject = requiredComponentObject.transform.GetChild(GRID_CENTER_INDEX);

        if(gameObject.CompareTag("Soldier")) {
            Vector3 awayDirection = gameObject.transform.position - gridCenterObject.transform.position;
            awayDirection.y = 0;
            Quaternion awayRotation = Quaternion.LookRotation(awayDirection);
            transform.rotation = awayRotation;
        }
    }

    private void Update() {
        if(gameObject.CompareTag("Enemy")) {
            transform.LookAt(gridCenterObject);
        }
    }
}
