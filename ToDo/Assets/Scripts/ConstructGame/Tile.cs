using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Tile : MonoBehaviour
{
    //private PlayerInput playerInput;
    private int tileID = 0;
    public int TileID { get { return tileID; } set { tileID = value; } }

    private void OnEnable()
    {
        Player.ObjectClicked += HandleObjectClick;
    }

    private void OnDisable()
    {
        Player.ObjectClicked -= HandleObjectClick;

    }

    private void HandleObjectClick(int ID)
    {
        if(tileID == ID)
        {
            Debug.Log("Found TIle");
        }
    }

    /*private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.actions["TouchPress"].started += ctx => StartTouch(ctx);
        playerInput.actions["TouchPress"].canceled += ctx => EndTouch(ctx);
    }

    private void StartTouch(InputAction.CallbackContext ctx)
    {
        Debug.Log("Touch Started"+ playerInput.actions["TouchPosition"].ReadValue<Vector2>());
        Player.myPlayer.CreateCubeOnSpot(transform.position);
    }

    private void EndTouch(InputAction.CallbackContext ctx)
    {
        Debug.Log("Touch Ended");
    }

    /*public void OnMouseDown()
    {
        Debug.Log(Time.time);
        Player.myPlayer.CreateCubeOnSpot(transform.position);
    }*/

}
