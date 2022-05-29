using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectSoldier : MonoBehaviour
{
    [SerializeField]
    private LayerMask soldierLayer;

    private float radius = 5f;

    private void FixedUpdate() {
        if(Physics.CheckSphere(transform.position, radius, soldierLayer)) {
            Debug.Log("Soldier Spotted!");
        }
    }

}
