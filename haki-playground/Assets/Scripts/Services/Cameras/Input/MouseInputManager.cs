using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Utility.InputService;
using UnityEngine;

[Service(typeof(InputService))]
public class MouseInputManager : CameraInputService
{
    Vector2Int screen;
    float mousePositionOnRotateStart;

    // Event
    public static event MoveInputHandler OnMoveInput;
    public static event RotateInputHandler OnRotateInput;
    public static event ZoomInputHandler OnZoomInput;

    private void Awake()
    {
        screen = new Vector2Int(Screen.width, Screen.height);
    }

    public void MousePosition()
    {
        Vector3 mp = Input.mousePosition;
        bool mouseValid = (mp.y <= screen.y * 1.05f && 
        mp.y >= screen.y * -0.05f && 
        mp.x <= screen.x * 1.05f && 
        mp.x >= screen.x * -0.05f);

        if (!mouseValid)return;

        //Mouse Camera Movement
        if (mp.y > screen.y * 0.95f)
        {
            OnMoveInput?.Invoke(Vector3.forward);
        }
        else if (mp.y < screen.y * 0.05f)
        {
            OnMoveInput?.Invoke(-Vector3.forward);
        }

        if (mp.x > screen.x * 0.95f)
        {
            OnMoveInput?.Invoke(Vector3.right);
        }
        else if (mp.x < screen.x * 0.05f)
        {
            OnMoveInput?.Invoke(-Vector3.right);
        }

        //Mouse Camera Rotation
        if (Input.GetMouseButtonDown(1))
        {
            mousePositionOnRotateStart = mp.x;
        }
        else if (Input.GetMouseButton(1))
        {
            if (mp.x < mousePositionOnRotateStart)
            {
                OnRotateInput?.Invoke(-1f);
            }
            else if (mp.x > mousePositionOnRotateStart)
            {
                OnRotateInput?.Invoke(1f);
            }
        }

        //Mouse Scroll Zoom
        if (Input.mouseScrollDelta.y > 0)
        {
            OnZoomInput?.Invoke(-3f);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            OnZoomInput?.Invoke(6f);
        }
    }

}