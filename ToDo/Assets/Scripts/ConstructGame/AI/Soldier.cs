using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab = null;

    private const float MAX_ROTATION = 30f;
    private const float MAX_TWEENTIME = 4f;
    private const float MIN_TWEENTIME = 2f;
    private const float RADIUS = 5f;


    private enum State
    {
        Scouting,
        ShootingTarget,
    }

    private State state;
    [Tooltip("Requires a trigger to detect enemy")]
    [SerializeField]
    private LayerMask enemyLayerMask;
    private List<GameObject> targets;
    private bool isTargetAcquired = false;

    Vector3 currentRotation;
    private float timer1 = 2f;
    float scoutDuration = 0f;

    private void Awake() {
        targets = new List<GameObject>();
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
                }
                timer1 -= Time.deltaTime;
                //Find Enemy using distance/trigger
                break;

            case State.ShootingTarget:
                //Begin shooting the acquired target
                transform.LookAt(targets[0].transform);
                //return to scouting once the target is destroyed
                break;
        }
    }


    private void Attack()
    {
        //instantiate projectile through other script
        //change state once the enemy is destroyed
    }

    private void Scout()
    {
        scoutDuration = Random.Range(MIN_TWEENTIME, MAX_TWEENTIME);
        currentRotation = transform.rotation.eulerAngles;
        LeanTween.rotateY(this.gameObject, currentRotation.y + Random.Range(-MAX_ROTATION, MAX_ROTATION), scoutDuration);

        if(Physics.CheckSphere(transform.position, RADIUS, enemyLayerMask)) {
            state = State.ShootingTarget;
            Debug.Log("Target found");

            Collider[] colliders = Physics.OverlapSphere(transform.position, RADIUS, enemyLayerMask);

            foreach(Collider collider in colliders) {
                targets.Add(collider.gameObject);
            }
        }
    }

}
