using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoute : MonoBehaviour
{
    [SerializeField] 
    private Transform[] routePoints;

    private Vector3 gizmoPosition;

    private void OnDrawGizmos() {
        for(float t = 0; t <= 1; t += 0.05f) {
            gizmoPosition = Mathf.Pow(1 - t, 3) * routePoints[0].position +
                            3 * Mathf.Pow(1 - t, 2) * t * routePoints[1].position +
                            3 * (1 - t) * Mathf.Pow(t, 2) * routePoints[2].position +
                            Mathf.Pow(t, 3) * routePoints[3].position;

            Gizmos.DrawSphere(gizmoPosition, 0.25f);
        }

        Gizmos.DrawLine(new Vector3(routePoints[0].position.x,
                                    routePoints[0].position.y,
                                    routePoints[0].position.z),
                        new Vector3(routePoints[1].position.x,
                                    routePoints[1].position.y,
                                    routePoints[1].position.z));

        Gizmos.DrawLine(new Vector3(routePoints[2].position.x,
                                    routePoints[2].position.y,
                                    routePoints[2].position.z),
                        new Vector3(routePoints[3].position.x,
                                    routePoints[3].position.y,
                                    routePoints[3].position.z));
    }
}
