using Assets.Scripts.Services.Core;
using UnityEngine;

namespace Assets.Scripts.Services.Utility.InputService
{
    public interface IInputService
    {
        bool IsRightMouseButtonDown { get; }
        bool IsRightMouseButtonUp { get; }

        bool IsLeftMouseButtonDown { get; }
        bool IsLeftMouseButtonUp { get; }
        Vector3 MousePosition { get; }

        void Update();
        bool IsKeyDown(KeyCode keyCode);
    }


    [Service(typeof(IInputService))]
    public class InputService : IInputService
    {
        private readonly InputState currentState;
        private InputState prevState;
        private class InputState
        {
            public bool IsRightMouseButtonDown { get; set; }
            public bool IsRightMouseButtonUp { get; set; }
            public bool IsLeftMouseButtonDown { get; set; }
            public bool IsLeftMouseButtonUp { get; set; }
            public Vector3 MousePosition { get; set; }
        }

        public bool IsRightMouseButtonDown => currentState.IsRightMouseButtonDown;
        public bool IsRightMouseButtonUp => currentState.IsRightMouseButtonUp;

        public bool IsLeftMouseButtonDown => currentState.IsLeftMouseButtonDown;
        public bool IsLeftMouseButtonUp => currentState.IsLeftMouseButtonUp;
        public Vector3 MousePosition => currentState.MousePosition;

        public InputService()
        {
            currentState = new InputState();
        }

        public void Update()
        {
            prevState = currentState; // this might be usefull later on.

            HandleInput();
        }

        public bool IsKeyDown(KeyCode keyCode)
        {
            return Input.GetKey(keyCode);
        }

        private void HandleInput()
        {
            currentState.IsRightMouseButtonDown = Input.GetMouseButton(1);
            currentState.IsRightMouseButtonUp = Input.GetMouseButtonUp(1);
            currentState.IsLeftMouseButtonDown = Input.GetMouseButton(0);
            currentState.IsLeftMouseButtonUp = Input.GetMouseButtonUp(0);

            currentState.MousePosition = Input.mousePosition;

        }
    }

    public class CameraInputService : MonoBehaviour
    {
        
        //Camera Input
        public delegate void MoveInputHandler(Vector3 moveVector);
        public delegate void RotateInputHandler(float rotateAmount);
        public delegate void ZoomInputHandler(float zoomAmount);

        private KeyboardInputManager kInput;
        private MouseInputManager mInput;

        public void Start()
        {
            kInput = GetComponent<KeyboardInputManager>();
            mInput = GetComponent<MouseInputManager>();
        }

        public void Update()
        {
            kInput.CameraMovement();
            mInput.MousePosition();
        }
    }
}

