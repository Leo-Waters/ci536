using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera cam;
    public float Xmin, Ymin;
    public float Xmax, Ymax;
    public float PanSpeed=1;
    Vector2 lastPanPosition;
    void Update()
    {

        if (Input.GetMouseButtonDown(0))//get mouse pos
        {
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))//pan the camera
        {
            PanCamera(Input.mousePosition);
        }
    }


    void PanCamera(Vector2 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector2 move = new Vector2(offset.x * PanSpeed,offset.y * PanSpeed);

        // Perform the movement
        transform.Translate(move, Space.World);

        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, Xmin, Xmax);
        pos.y = Mathf.Clamp(transform.position.y, Ymin, Ymax);
        transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }
}
