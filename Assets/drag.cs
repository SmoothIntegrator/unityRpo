using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragmouse : MonoBehaviour

{
    private Camera cam;
    private bool isDragging = false;
    private Vector3 offset;

    void Start()
    {
        // Define the camera
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        isDragging = true;

        // Convert object position and mouse position to world space
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = transform.position.z;

        // Store offset so object doesn't snap to mouse center
        offset = transform.position - mouseWorldPos;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            // Convert mouse screen position to world position
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = transform.position.z;

            // Apply offset and move object
            transform.position = mouseWorldPos + offset;
        }
    }
}
