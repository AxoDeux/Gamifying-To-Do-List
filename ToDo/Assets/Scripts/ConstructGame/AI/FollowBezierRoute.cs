using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBezierRoute : MonoBehaviour
{
    private const string REQUIRED_OBJECT = "EnemyRequiredComponents";
    private const int ROUTE1_INDEX = 1;
    private const int ROUTE2_INDEX = 2;
    private const float SPEED = 0.2f;

    [SerializeField]
    private Transform[] routes;

    private int routeToGo;

    private float tParam;

    private Vector3 objectPosition;

    private float speed;
    private bool isCoroutineAllowed;
    private bool isDelayed = false;

    private void Awake() {
        routes = new Transform[2];
    }

    private void Start() {
        GameObject requiredComponentObject = GameObject.Find(REQUIRED_OBJECT);
        routes[0] = requiredComponentObject.transform.GetChild(ROUTE1_INDEX);
        routes[1] = requiredComponentObject.transform.GetChild(ROUTE2_INDEX);

        routeToGo = 0;
        tParam = 0f;
        isCoroutineAllowed = true;
    }

    private void Update() {
        if(!isDelayed) {
            StartCoroutine(StartDelay());
        }

        if(isCoroutineAllowed && isDelayed) {
            StartCoroutine(FollowRoute(routeToGo));
        }
    }

    private IEnumerator FollowRoute(int routeNum) {
        isCoroutineAllowed = false;

        Vector3 p0 = routes[routeNum].GetChild(0).position;
        Vector3 p1 = routes[routeNum].GetChild(1).position;
        Vector3 p2 = routes[routeNum].GetChild(2).position;
        Vector3 p3 = routes[routeNum].GetChild(3).position;

        while(tParam < 1) {
            tParam += Time.deltaTime * SPEED;

            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                            3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                            3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2+
                            Mathf.Pow(tParam, 3) * p3;

            transform.position = objectPosition;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;
        routeToGo++;

        if(routeToGo > routes.Length - 1) {
            routeToGo = 0;
        }

        isCoroutineAllowed = true;
    }

    private IEnumerator StartDelay() {
        yield return new WaitForSeconds(Random.Range(0f, 5f));
        isDelayed = true;
    }
}
