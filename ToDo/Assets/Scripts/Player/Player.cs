using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player myPlayer;

    public static event Action<int> ObjectClicked;
    public static event Action<Soldier> OnSoldierSpawned;
    public static event Action<Soldier> OnSoldierDespawned;


    [SerializeField] private GameObject[] elementsArray;
    private int activeElement = 0;

    [SerializeField] private TMP_Text[] debugTexts;

    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private GameObject gridCenter = null;
    [SerializeField] private float distanceToTarget = 0f;

    private Vector3 spawnPoint = new Vector3(0,0,0);

    private bool isMoving = false;
    private Vector3 previousPos = new Vector3();
    private int fingersDown = 0;
    private float newDistance = 0f;
    private float prevDistance = 0f;
    private bool twoFingersUsed = false;

    private bool isConstructing = true;

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
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += HandleFingerUp;       

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += HandleFingerMove;       

    }

    private void OnDisable()
    {
        //playerInput.actions["TouchPress"].started -= ctx => StartTouch(ctx);
        // playerInput.actions["TouchPress"].canceled -= ctx => EndTouch(ctx);

        //characterInputs.Disable();

        EnhancedTouchSupport.Disable();
        //TouchSimulation.Disable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= HandleFingerDown;       //unity engine also has 'Touch'
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= HandleFingerUp;

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove -= HandleFingerMove;

    }

    private void Update()
    {
        debugTexts[0].text = $"Active fingers: {UnityEngine.InputSystem.EnhancedTouch.Touch.activeFingers.Count}";
    }

    #endregion

    #region EnhancedTouchEventHandlers

    private void HandleFingerDown(Finger finger)
    {
        //fingersDown++;
        //fingers.Add(finger);

        previousPos = mainCamera.ScreenToViewportPoint(finger.screenPosition);
    }

    private void HandleFingerUp(Finger finger)
    {
        if(UnityEngine.InputSystem.EnhancedTouch.Touch.activeFingers.Count != 1) { return; }
        GetSpawnPoint(finger);
        isMoving = false;
        twoFingersUsed = false;
        prevDistance = 0f;
        //fingersDown--;
        //fingers.Remove(finger);
    }

    private void HandleFingerMove(Finger finger)
    {
        Vector3 newPos = mainCamera.ScreenToViewportPoint(finger.screenPosition);
        Vector3 direction = previousPos - newPos;

        if (direction.sqrMagnitude < 0.001) {return;}

        isMoving = true;

        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeFingers.Count == 1 && !twoFingersUsed)
        {
            //rotate along y axis

            float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
            float rotationAroundXAxis = direction.y * 180; // camera moves vertically

            mainCamera.transform.position = gridCenter.transform.position;

            mainCamera.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
            mainCamera.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World); // <— This is what makes it work!

            mainCamera.transform.Translate(new Vector3(0, 0, -mainCamera.orthographicSize));

            previousPos = newPos;
            debugTexts[1].text = $"Distance to target: {distanceToTarget}";


        }

        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeFingers.Count == 2)
        {
            //zoom in/out

            newDistance = Vector2.Distance(UnityEngine.InputSystem.EnhancedTouch.Touch.activeFingers[0].screenPosition,
                                            UnityEngine.InputSystem.EnhancedTouch.Touch.activeFingers[1].screenPosition);

            if (newDistance > prevDistance)     //zoom out
            {
               // if(mainCamera.orthographicSize <= 5) { return; }
                mainCamera.orthographicSize -= 0.01f;
            }

            else if (newDistance < prevDistance)       //zoom in
            {
                //if (mainCamera.orthographicSize >= 10) { return; }
                mainCamera.orthographicSize += 0.01f;
            }

            debugTexts[2].text = $"NewDist is greater than PrevDist : {prevDistance<newDistance}";
            debugTexts[1].text = $"Distance from target: {mainCamera.orthographicSize}";

            prevDistance = newDistance;
            twoFingersUsed = true;
        }
    }

    #endregion

    private void GetSpawnPoint(Finger finger)
    {
        if (!isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(finger.screenPosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.gameObject.CompareTag("Tile") && isConstructing)
                {
                    GameObject selectedTile = hitInfo.collider.gameObject;
                    //Debug.Log($"Tile ID is {selectedTile.GetComponent<Tile>().TileID}");
                    ObjectClicked?.Invoke(selectedTile.GetComponent<Tile>().TileID);

                    spawnPoint = selectedTile.transform.position + new Vector3(0,
                        selectedTile.GetComponent<BoxCollider>().size.y / 2 + elementsArray[0].GetComponent<BoxCollider>().size.y / 2,
                        0);     //Element must spawn above the tile
                    CreateCubeOnSpot(spawnPoint);
                }

                if (hitInfo.collider.gameObject.CompareTag("Element"))
                {
                    GameObject selectedObject = hitInfo.collider.gameObject;

                    if (!isConstructing) 
                    { 
                        DestructElement(selectedObject);
                        return;
                    }
                    //Debug.Log($"Element is Spawned : {selectedObject.name}");

                    CheckDirectionToCreate(hitInfo.normal, selectedObject);
                }
            }
        }
    }

    public void CreateCubeOnSpot(Vector3 spawnHere)
    {
        Instantiate(elementsArray[activeElement], spawnHere, Quaternion.identity);

        if(elementsArray[activeElement].TryGetComponent<Soldier>(out Soldier soldier)) {
            OnSoldierSpawned.Invoke(soldier);
        }
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

    private void DestructElement(GameObject element)
    {
        if(element.TryGetComponent<Soldier>(out Soldier soldier)) {
            OnSoldierDespawned.Invoke(soldier);
        }
        Destroy(element);
    }


    public void OnElementSelect(int elementNum)
    {
        activeElement = elementNum;
    }

    public void DestructElements()
    {
        isConstructing = false;
    }

    public void ConstructElements()
    {
        isConstructing = true;
    }

    


    //Older Code--------------------------------------
    /*
    private void Start()
    {
        characterInputs.Player.TouchPress.started += context => StartTouch(context);
    
    }

    private void StartTouch(InputAction.CallbackContext ctx)
    {

        //float x = playerInput.actions["TouchPosition"].ReadValue<Vector2>().x;
        //float z = playerInput.actions["TouchPosition"].ReadValue<Vector2>().y;
        Debug.Log("Checking");
        Ray ray = Camera.main.ScreenPointToRay(characterInputs.Player.TouchPosition.ReadValue<>());
    
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
    }
    */
}
