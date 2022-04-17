using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem buildingSystem;

    public GridLayout gridLayout;
    private Grid grid;

    [SerializeField] private Tilemap tileMap;
    [SerializeField] private TileBase tileBase;

    [SerializeField] private GameObject prefab1;

    //private PlaceableObject objectToPlace;

    //Input
    private PlayerInput playerInput;
    private InputAction interactAction;


    private void Awake()
    {
        buildingSystem = this;
        grid = gridLayout.GetComponent<Grid>();
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"];
    }

    private void OnEnable()
    {
        interactAction.performed += HandleInteraction;
    }

    private void OnDisable()
    {
        interactAction.performed -= HandleInteraction;
    }


    private void HandleInteraction(InputAction.CallbackContext context)
    {
        InitializeWithObject(prefab1);
    }

    public static Vector3 GetMouseWorldPosition()
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out hitInfo))
        {
            return hitInfo.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);      //converts a world pos to cell pos.
        position = grid.GetCellCenterWorld(cellPos);                //Get the logical center coordinate of a grid cell in world space
        return position;
    }

    ///Building Placement
    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        //objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
    }
}
