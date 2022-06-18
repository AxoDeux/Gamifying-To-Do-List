using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerEnter : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile = null;

    [SerializeField]
    private float force = 2f;

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Hello!");

        GameObject ball = Instantiate(projectile, transform.parent);
        ball.GetComponent<Rigidbody>().AddForce(Vector3.up * force);
    }
}
