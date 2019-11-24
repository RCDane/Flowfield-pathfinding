using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCamera : MonoBehaviour
{
    string debugMessage = "CameraMove";
    bool Moving;
    Camera cam;
    // Use this for initialization
    void Start()
    {
        cam = Camera.main;
        DebugManager.AddDebugName(debugMessage);
    }

    Vector2 offset;
    Vector3 camStart;
    // Update is called once per frame
    void Update()
    {
        bool camMove = DebugManager.Check(debugMessage);

        if (Input.GetKey(KeyCode.Space) && camMove)
        {
			Scroll();
        }

        if (Input.GetMouseButtonDown(0) && camMove)
        {
            print("buttonDown");
            Moving = true;
            Vector2 camScreenPos = new Vector2(Screen.width / 2, Screen.height / 2);
            Vector2 mousePos = Input.mousePosition;
            // if (oldMousepos == null)
            camStart = cam.transform.position;
            Vector2 worldMouse = cam.ScreenToWorldPoint(mousePos);
            startMousePos = Input.mousePosition;

        }
        else if (Input.GetMouseButton(0) && camMove)
        {
            // print("moving");
            Move();
        }
        else if (Input.GetMouseButtonUp(0) && camMove)
        {
            // print("Release");
            Moving = false;
        }
    }

    void Scroll()
    {
        float scrollAmount = Input.mouseScrollDelta.y;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
			scrollAmount = 0;
			scrollAmount += Input.GetKey(KeyCode.UpArrow) ? 0 : 0.05f;
			scrollAmount += Input.GetKey(KeyCode.DownArrow) ? 0 : -0.05f;
        }
        // Input.simulateMouseWithTouches = true;
        if (cam == null)
            cam = Camera.main;

        cam.orthographicSize += scrollAmount;
    }
    Vector2 startMousePos;
    Vector2 oldMousepos;
    void Move()
    {

        Vector2 mousePos = Input.mousePosition;
        if (oldMousepos == null)
            oldMousepos = Input.mousePosition;
        Vector2 worldMouse = cam.ScreenToWorldPoint(mousePos);
        Vector2 start = cam.ScreenToWorldPoint(startMousePos);
        Vector2 diff = worldMouse - start;
        // print("worldPos: " + worldMouse + "   Diff:  " + diff + "   CamStart: " + camStart);
        oldMousepos = mousePos;
        cam.transform.position = camStart - (Vector3)diff;

    }
}
