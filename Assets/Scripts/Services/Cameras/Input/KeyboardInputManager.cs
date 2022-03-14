using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Utility.InputService;
using UnityEngine;

[Service(typeof(InputService))]
public class KeyboardInputManager : CameraInputService
{
    public static event MoveInputHandler OnMoveInput;
    public static event RotateInputHandler OnRotateInput;
    public static event ZoomInputHandler OnZoomInput;

    public void CameraMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float rotation = Input.GetAxis("Rotate");

        //Move Camera Input
        if (x > 0)
        {
            OnMoveInput?.Invoke(Vector3.right);
        }
        if (x < 0)
        {
            OnMoveInput?.Invoke(-Vector3.right);
        }

        if (z > 0)
        {
            OnMoveInput?.Invoke(Vector3.forward);
        }
        if (z < 0)
        {
            OnMoveInput?.Invoke(-Vector3.forward);
        }

        //Rotate Camera Input
        if (rotation > 0)
        {
            OnRotateInput?.Invoke(-1f);
        }
        if (rotation < 0)
        {
            OnRotateInput?.Invoke(1f);
        }

        //Zoom Camera Input
        if (Input.GetKey(KeyCode.Z))
        {
            OnZoomInput?.Invoke(-1f);
        }
        if (Input.GetKey(KeyCode.X))
        {
            OnZoomInput?.Invoke(1f);
        }
    }

}
