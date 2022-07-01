using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerClickHandler
{
    private Vector3 offset;

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = BuildingSystem.GetMouseWorldPosition() + offset;
        transform.position = BuildingSystem.buildingSystem.SnapCoordinateToGrid(pos);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        offset = transform.position - BuildingSystem.GetMouseWorldPosition();
    }

    private void OnMouseDown()
    {
        Debug.Log("Dragging");
        offset = transform.position - BuildingSystem.GetMouseWorldPosition();       //Difference in cube pos and its world position (called once)
    }

    private void OnMouseDrag()
    {
        Vector3 pos = BuildingSystem.GetMouseWorldPosition() + offset;              //Adding the offset to the world postion during drag operation (called while dragging)
        transform.position = BuildingSystem.buildingSystem.SnapCoordinateToGrid(pos);
    }
}
