using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridForm : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;

    private float xPos = 0f;
    private float zPos = 0f;

    private int tileCount = 1;

    private void Awake()
    {
        for(int i=0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject obj = Instantiate(tilePrefab, new Vector3(xPos, 0f, zPos), Quaternion.identity, this.transform);
                Tile tileInstance = obj.GetComponent<Tile>();
                tileInstance.TileID= tileCount;
                tileCount++;
                xPos++;
            }
            zPos++;
            xPos = 0;
        }
    }
}
