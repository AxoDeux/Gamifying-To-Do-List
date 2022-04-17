using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class Player : MonoBehaviour
{
    public static Player myPlayer;

    public static event Action<int> ObjectClicked;

    [SerializeField] private GameObject[] elementsArray;
    private int activeElement = 0;

    private Vector3 spawnPoint = new Vector3(0,0,0);

    //private CharacterInputs characterInputs;
    //private PlayerInput playerInput;

        #region Unity Methods

    private void Awake()
    {
        myPlayer = this;
        //playerInput = GetComponent<PlayerInput>();

        //characterInputs = new CharacterInputs();
    }
    private void OnEnable()
    { 
        //characterInputs.Enable();

        EnhancedTouchSupport.Enable();
        //TouchSimulation.Enable();

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += HandleFingerDown;       //unity engine also has 'Touch'

    }

    private void OnDisable()
    {
        //playerInput.actions["TouchPress"].started -= ctx => StartTouch(ctx);
        // playerInput.actions["TouchPress"].canceled -= ctx => EndTouch(ctx);

        //characterInputs.Disable();

        EnhancedTouchSupport.Disable();
        //TouchSimulation.Disable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= HandleFingerDown;       //unity engine also has 'Touch'

    }

    #endregion

    private void HandleFingerDown(Finger finger)
    {
        Ray ray = Camera.main.ScreenPointToRay(finger.screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.gameObject.CompareTag("Tile"))
            {
                GameObject selectedTile = hitInfo.collider.gameObject;
                Debug.Log($"Tile ID is {selectedTile.GetComponent<Tile>().TileID}");
                ObjectClicked?.Invoke(selectedTile.GetComponent<Tile>().TileID);

                spawnPoint = selectedTile.transform.position + new Vector3(0, 
                    selectedTile.GetComponent<BoxCollider>().size.y / 2 + elementsArray[0].GetComponent<BoxCollider>().size.y/2,
                    0);     //Element must spawn above the tile
                CreateCubeOnSpot(spawnPoint);
            }

            if (hitInfo.collider.gameObject.CompareTag("Element"))
            {
                GameObject selectedObject = hitInfo.collider.gameObject;
                Debug.Log($"Element is Spawned : {selectedObject.name}");

                CheckDirectionToCreate(hitInfo.normal, selectedObject);
            }
        }
    }

    public void CreateCubeOnSpot(Vector3 spawnHere)
    {
        Instantiate(elementsArray[activeElement], spawnHere, Quaternion.identity);
    }

    public void SetActiveElement(int elementNum)
    {
        activeElement = elementNum;
    }

    private void CheckDirectionToCreate(Vector3 faceNormal, GameObject refObj)
    {
        if(faceNormal.x == 1 || faceNormal.x == -1)
        {
            spawnPoint = refObj.transform.position;
            spawnPoint.x += refObj.GetComponent<BoxCollider>().size.x * faceNormal.x;
        }

        if (faceNormal.y == 1 || faceNormal.y == -1)
        {
            spawnPoint = refObj.transform.position;
            spawnPoint.y += refObj.GetComponent<BoxCollider>().size.y * faceNormal.y;
        }

        if (faceNormal.z == 1 || faceNormal.z == -1)
        {
            spawnPoint = refObj.transform.position;
            spawnPoint.z += refObj.GetComponent<BoxCollider>().size.z * faceNormal.z;
        }

        CreateCubeOnSpot(spawnPoint);
    }

    //Older Code--------------------------------------

    /*private void Start()
{
    characterInputs.Player.TouchPress.started += context => StartTouch(context);

}*/

    /*private void StartTouch(InputAction.CallbackContext ctx)
{

    //float x = playerInput.actions["TouchPosition"].ReadValue<Vector2>().x;
    //float z = playerInput.actions["TouchPosition"].ReadValue<Vector2>().y;
    Debug.Log("Checking");
    Ray ray = Camera.main.ScreenPointToRay(characterInputs.Player.TouchPosition.ReadValue<Vector2>());

    if (Physics.Raycast(ray, out RaycastHit hitInfo))
    {
        Debug.Log("!!");

        if (hitInfo.collider.gameObject.CompareTag("Tile"))
        {
            Debug.Log("Error!!");

            Debug.Log($"Tile ID is {hitInfo.collider.gameObject.GetComponent<Tile>().TileID}");
            ObjectClicked?.Invoke(hitInfo.collider.gameObject.GetComponent<Tile>().TileID);
        }
    }
}*/
}
