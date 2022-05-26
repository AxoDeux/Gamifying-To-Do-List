using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab = null;

    private const float MAX_ROTATION = 30f;
    private const float MAX_TWEENTIME = 4f;
    private const float MIN_TWEENTIME = 2f;

    private enum State
    {
        Scouting,
        ShootingTarget,
    }

    private State state;
    [Tooltip("Requires a trigger to detect enemy")]
    private GameObject target;

    private bool isTargetAcquired = false;

    private float timer1 = 2f;

    private void Start()
    {
        state = State.Scouting;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Scouting:
                //Add soldier rotation to show scouting
                if(timer1 < 0)
                {
                    timer1 = Scout();
                }
                timer1 -= Time.deltaTime;
                //Find Enemy using distance/trigger
                break;

            case State.ShootingTarget:
                //Begin shooting the acquired target
                //return to scouting once the target is destroyed
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Enemy")) { return; }

        Vector3 enemyPos = other.gameObject.transform.position;

        if(!isTargetAcquired &&
            Vector3.Dot(enemyPos-transform.position, transform.position)>0)
        {
            isTargetAcquired = true;
            target = other.gameObject;
            state = State.Scouting;
            Attack();

        }
    }

    private void Attack()
    {
        //instantiate projectile through other script
        //change state once the enemy is destroyed
    }

    private float Scout()
    {
        //transform.Rotate(new Vector3(0f, Random.Range(-30, 30), 0f));
        float scoutDuration = Random.Range(MIN_TWEENTIME, MAX_TWEENTIME);
        LeanTween.rotateY(this.gameObject, Random.Range(-MAX_ROTATION, MAX_ROTATION), scoutDuration);

        return scoutDuration;
    }

}
