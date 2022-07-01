using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridForm : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab = null;
    [SerializeField] private GameObject gridCenter = null;
    [SerializeField] private int gridSize = 0;

    private float xPos = 0f;
    private float zPos = 0f;

    private int tileCount = 1;

    private void Awake()
    {
        for(int i=0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                GameObject obj = Instantiate(tilePrefab, new Vector3(xPos, 0f, zPos), Quaternion.identity, this.transform);
                obj.GetComponent<Tile>().TileID = tileCount;
                tileCount++;
                xPos++;
            }
            zPos++;
            xPos = 0;
        }
    }

    private void Start()
    {
        gridCenter.transform.position = new Vector3(gridSize / 2, 0, gridSize / 2);

        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}
