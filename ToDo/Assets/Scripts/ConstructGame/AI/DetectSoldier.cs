using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectSoldier : MonoBehaviour
{
    private const string REQUIRED_OBJECT = "EnemyRequiredComponents";
    private const int GRID_CENTER_INDEX = 0;
    private const float RADIUS = 5f;

    [SerializeField]
    private LayerMask soldierLayer;

    [SerializeField]
    private Transform gridCenterObject = null;

    private GameObject requiredComponentObject;
    private FollowBezierRoute bezierMovementScript;

    private bool isSoldierFound = false;
    private List<GameObject> soldiers;

    private void Awake() {
        bezierMovementScript = GetComponent<FollowBezierRoute>();
        soldiers = new List<GameObject>();
    }

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
        if(isSoldierFound) {
            transform.LookAt(soldiers[0].transform);
        } else {
            transform.LookAt(gridCenterObject);
        }
    }

    private void FixedUpdate() {
        if(Physics.CheckSphere(transform.position, RADIUS, soldierLayer) && !isSoldierFound) {              //Check if soldier is in range and get the soldier gameobjects in range list
            //bezierMovementScript.enabled = false;
            isSoldierFound = true;

            Collider[] colliders = Physics.OverlapSphere(transform.position, RADIUS, soldierLayer);
            foreach(Collider collider in colliders) {
                soldiers.Add(collider.gameObject);
                Debug.Log("Got Soldier Object");
            }
        }
    }

}
