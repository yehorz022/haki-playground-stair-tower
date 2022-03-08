using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Utility.InputService;
using UnityEngine;

namespace Assets.Scripts.Services.Cameras
{
    public interface ICameraService
    {
        Ray CreateMouseRay();
    }

    [Service(typeof(ICameraService))]
    public class CameraService : ICameraService
    {
        private readonly IInputService inputService;

        public CameraService(IInputService inputService)
        {
            this.inputService = inputService;
        }

        public Ray CreateMouseRay()
        {
            return Camera.main.ScreenPointToRay(inputService.MousePosition);
        }
    }
}