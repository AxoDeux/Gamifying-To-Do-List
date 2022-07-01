using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const string ENEMY_TAG = "Enemy";

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag(ENEMY_TAG)) {
            //Collision event?
            Destroy(gameObject);
        }
    }
}
