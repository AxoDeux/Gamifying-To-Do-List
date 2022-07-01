using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    private const string PROJECTILE_TAG = "Projectile";
    private const float MAX_ROTATION = 30f;
    private const float MAX_TWEENTIME = 4f;
    private const float MIN_TWEENTIME = 2f;
    private const float RADIUS = 5f;

    public delegate void OnSoldierKilledEventHandler(GameObject soldier);
    public static event OnSoldierKilledEventHandler OnSoldierKilledEvent;
    private enum State
    {
        Scouting,
        ShootingTarget,
    }

    private State state;

    [Tooltip("Requires a trigger to detect enemy")]
    [SerializeField]
    private LayerMask enemyLayerMask;

    [SerializeField]
    private GameObject projectilePrefab = null;

    [SerializeField]
    private GameObject projectileSpawnPoint = null;

    [SerializeField]
    private float force = 4f;

    private List<GameObject> targets;
    private GameObject target = null;
    private bool isTargetAcquired = false;

    Vector3 currentRotation;
    private int hit = 0;
    private float timer1 = 2f;
    private float timer2 = 2f;

    float scoutDuration = 0f;

    private void Awake() {
        targets = new List<GameObject>();
    }

    private void OnEnable() {
        Enemy.OnEnemyKilled += HandleEnemyKilled;
    }

    private void OnDisable() {
        Enemy.OnEnemyKilled -= HandleEnemyKilled;
    }

    private void Start()
    {
        state = State.Scouting;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Scouting:
                //Add soldier rotation to show scouting
                if(timer1 < 0)
                {
                    Scout();
                    timer1 = 2f;
                }
                timer1 -= Time.deltaTime;
                //Find Enemy using distance/trigger
                break;

            case State.ShootingTarget:
                //Begin shooting the acquired target
                transform.LookAt(target.transform);
                if(timer2 < 0) {
                    Attack();
                    timer2 = 2f;
                }
                timer2 -= Time.deltaTime;
                //return to scouting once the target is destroyed
                break;
        }
    }


    private void Attack()
    {
        //instantiate projectile through other script
        //change state once the enemy is destroyed
        Vector3 direction = target.transform.position - projectileSpawnPoint.transform.position;
        direction /= direction.magnitude;

        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, Quaternion.identity);
        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
        projectileRB.AddForce(direction * force);

        if(!target) {
            state = State.Scouting;
        }

    }

    private void Scout()
    {
        scoutDuration = Random.Range(MIN_TWEENTIME, MAX_TWEENTIME);
        currentRotation = transform.rotation.eulerAngles;
        LeanTween.rotateY(this.gameObject, currentRotation.y + Random.Range(-MAX_ROTATION, MAX_ROTATION), scoutDuration);

        if(Physics.CheckSphere(transform.position, RADIUS, enemyLayerMask)) {

            Collider[] colliders = Physics.OverlapSphere(transform.position, RADIUS, enemyLayerMask);

            foreach(Collider collider in colliders) {
                targets.Add(collider.gameObject);
            }

            target = targets.ToArray()[0];
            state = State.ShootingTarget;
        }       

    }

    private void HandleEnemyKilled(GameObject enemyObject) {
        if(target == enemyObject) {
            targets.Remove(enemyObject);
            target = null;
            state = State.Scouting;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag(PROJECTILE_TAG)) {
            hit++;
            if(hit >= 2) {
                OnSoldierKilledEvent.Invoke(gameObject);
                Destroy(gameObject);
            }
        }
    }

}
