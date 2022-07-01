using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(PlayerInput))]
public class InteractObject1 : MonoBehaviour
{
    [SerializeField] private Canvas instructionCanvas = null;
    [SerializeField] private Canvas inspectCanvas = null;

    private PlayerInput playerInput;
    private InputAction interactAction;

    private void Awake()
    {
        playerInput = gameObject.GetComponent<PlayerInput>();
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

    private void HandleInteraction(InputAction.CallbackContext obj)
    {
        inspectCanvas.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        instructionCanvas.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        instructionCanvas.gameObject.SetActive(false);
    }



}
