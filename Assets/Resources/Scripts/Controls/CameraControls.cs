using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour {
    private float DragSpeed = 1f;
    private float ZoomSpeed = 2f;
    private int MaxZoom = 10;
    private int MinZoom = 3;
    private Vector3 deltaPos;
    private Vector3 lastPos;
    public  Vector3 MoveDir;
    private float Zoom = 5;

    private Transform CameraTransform;


    void Start()
    {
        CameraTransform = transform.GetChild(0);
        transform.position = new Vector3(0,Zoom,0);
    }

    void Update()
    {
        Zoom -= ZoomSpeed * Input.GetAxis("Mouse ScrollWheel");
        Zoom = Mathf.Clamp(Zoom, MinZoom, MaxZoom);
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, Zoom, transform.position.z), ZoomSpeed * Time.deltaTime);
        if (Input.GetMouseButtonDown(1))
        {
            lastPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            deltaPos = Input.mousePosition - lastPos;
            transform.Translate(-deltaPos.x * DragSpeed * Time.deltaTime, 0.0f ,-deltaPos.y * DragSpeed * Time.deltaTime);
            lastPos = Input.mousePosition;
        }
    }
}
