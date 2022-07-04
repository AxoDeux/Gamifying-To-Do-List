using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    private const string PROJECTILE_TAG = "Projectile";
    private const float RADIUS = 5f;


    [SerializeField]
    private LayerMask soldierLayer;

    [SerializeField]
    private Transform gridCenterObject = null;

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float force = 4f;

    public delegate void OnEnemyKilledEventHandler(GameObject enemyObject);
    public static event OnEnemyKilledEventHandler OnEnemyKilled;

    private bool isSoldierFound = false;
    private List<GameObject> soldiers;
    private GameObject projectileSpawnPoint;
    private GameObject target;
    

    private enum State {
        Scouting,
        ShootingSoldier,
        ShootingObject
    }
    private State state;

    private int hit = 0;
    private float timer1 = 1f;
    private float timer2 = 2f;
    private float timer3 = 3f;


    private void Awake() {
        Transform go = transform.Find("EnemyObject");
        projectileSpawnPoint = go.Find("ProjectileSpawnPoint").gameObject;

        soldiers = new List<GameObject>();

        gridCenterObject = GameObject.Find("GridCenter").transform;

        if(go == null || projectileSpawnPoint == null || gridCenterObject == null) {
            Debug.LogWarning($"Couldn't find reference of either of projectileSpawnPoint, gridCenterObject in Enemy.cs script");
        }
    }

    private void OnEnable() {
        Soldier.OnSoldierKilledEvent += HandleSoldierKilled;
    }
    private void OnDisable() {
        Soldier.OnSoldierKilledEvent -= HandleSoldierKilled;
    }
    private void Update() {
        if(isSoldierFound) {
            transform.LookAt(target.transform);
        } else {
            transform.LookAt(gridCenterObject);
        }
    }

    private void FixedUpdate() {
        switch(state) {
            case State.Scouting:
                if(timer1 < 0) {
                    Scouting();
                    timer1 = 1f;
                }
                timer1 -= Time.deltaTime;
                break;

            case State.ShootingSoldier:
                if(timer2 < 0) {
                    ShootSoldier();
                    timer2 = 2f;
                }
                timer2 -= Time.deltaTime;
                break;

            case State.ShootingObject:
                if(timer3< 0) {
                    ShootObject();
                    timer3 = 2f;
                }
                timer3 -= Time.deltaTime;
                break;
        }
    }
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

    private void Scouting() {

        if(Physics.CheckSphere(transform.position, RADIUS, soldierLayer) && !isSoldierFound) {              //Check if soldier is in range and get the soldier gameobjects in range list
            //bezierMovementScript.enabled = false;
            isSoldierFound = true;
            soldiers.Clear();

            Collider[] colliders = Physics.OverlapSphere(transform.position, RADIUS, soldierLayer);
            foreach(Collider collider in colliders) {
                soldiers.Add(collider.gameObject);
            }

            GameObject[] soldiersArray = soldiers.ToArray();
            target = soldiersArray[0];
            state = State.ShootingSoldier;
        }
    }

    private void ShootSoldier() {

        Vector3 direction = target.transform.position - projectileSpawnPoint.transform.position;
        direction /= direction.magnitude;
        GameObject go = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, Quaternion.identity);
        go.GetComponent<Rigidbody>().AddForce(direction*force);

        if(!target) {
            state = State.Scouting;
        }
    }

    private void ShootObject() {
        //Check if soldier is not target
        //find the special element
        //position this enemy in proximity
        //start shooting the special object.
    }
    
    private void HandleSoldierKilled(GameObject soldier) {
        if(target != soldier) {return; }

        isSoldierFound = false;
        state = State.Scouting;
    }
}
