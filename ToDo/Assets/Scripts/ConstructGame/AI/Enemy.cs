using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    private const string PROJECTILE_TAG = "Projectile";

    public delegate void OnEnemyKilledEventHandler(GameObject enemyObject);
    public static event OnEnemyKilledEventHandler OnEnemyKilled;

    private int hit = 0;

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag(PROJECTILE_TAG)) {
            hit++;
            if(hit >= 2) {
                //Enemy Killed Event
                OnEnemyKilled.Invoke(gameObject);
                Destroy(gameObject);
            }
        }
    }
}
