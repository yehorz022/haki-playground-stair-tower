using UnityEngine;

namespace Assets.Services.InputService
{
    public interface IInputService
    {
        bool IsRightMouseButtonDown { get; }
        bool IsRightMouseButtonUp { get; }

        bool IsLeftMouseButtonDown { get; }
        bool IsLeftMouseButtonUp { get; }
        Vector3 MousePosition { get; }

        void Update();
    }

    [Service(typeof(IInputService))]
    public class InputService : IInputService
    {
        private InputState currentState;
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

            Handleinput();
        }

        private void Handleinput()
        {
            currentState.IsRightMouseButtonDown = Input.GetMouseButtonDown(1);
            currentState.IsRightMouseButtonUp = Input.GetMouseButtonUp(1);
            currentState.IsLeftMouseButtonDown = Input.GetMouseButtonDown(0);
            currentState.IsLeftMouseButtonUp = Input.GetMouseButtonUp(0);

            currentState.MousePosition = Input.mousePosition;

        }
    }
}

