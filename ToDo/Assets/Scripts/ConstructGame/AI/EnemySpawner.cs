using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] EnemyPrefabs = null;

    private List<Transform> spawnTransforms;
    private List<Vector3> spawnPoints; 

    private void Awake() {
        Transform[] childTransforms = GetComponentsInChildren<Transform>();
        spawnTransforms = new List<Transform>();
        for(int i = 0; i < childTransforms.Length; i++) {
            if(childTransforms[i] == null) { continue; }
            spawnTransforms.Add(childTransforms[i]);
        }

        spawnPoints = new List<Vector3>();

        for(int i = 0; i<spawnTransforms.Count; i++) {
            spawnPoints.Add(spawnTransforms[i].position);
        }
        spawnPoints.Remove(Vector3.zero);
    }

    public void OnFightClicked() {
        for(int i = 0; i < spawnPoints.Count; i++) {
            Instantiate(EnemyPrefabs[0], spawnTransforms[i + 1]);
        }
    }
}
