using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Utility.InputService;
using UnityEngine;

namespace Assets.Scripts.Services.Cameras
{

    public interface IMaterialService
    {
        Material MouseOver { get; }
        Material Selected { get; }

        void SetMaterials(Material mouseover, Material selected);
    }

    [Service(typeof(IMaterialService))]
    public class MaterialService : IMaterialService
    {
        /// <inheritdoc />
        public Material MouseOver { get; private set; }

        /// <inheritdoc />
        public Material Selected { get; private set; }

        /// <inheritdoc />
        public void SetMaterials(Material mouseover, Material selected)
        {
            MouseOver = mouseover;
            Selected = selected;
        }
    }

    public interface ICameraService
    {
        Ray CreateMouseRay();
        Vector3 Position { get; }
        Vector3 Heading { get; }
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

        /// <inheritdoc />
        public Vector3 Position => Camera.main.transform.position;

        public Vector3 Heading => Camera.main.transform.forward;
    }
}