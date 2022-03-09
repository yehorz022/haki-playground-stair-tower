using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Tools.Selector.Face;
using UnityEngine;

namespace Assets.Scripts.RunMode
{
    public class OnSelectedService : IOnSelected
    {
        private readonly GameObject go;

        public OnSelectedService(GameObject go)
        {
            this.go = go;
        }
        public void OnSelected(bool value)
        {
            go.SetActive(value);
        }

            
    }

    [Service(typeof(ISelected<ScaffoldingAssembly>))]
    public class SelectedAssembly : ISelected<ScaffoldingAssembly>
    {
        private readonly IOnSelected onSelected;
        private ScaffoldingAssembly assembly;

        public SelectedAssembly(IOnSelected onSelected)
        {
            this.onSelected = onSelected;
        }

        public void SetSelected(ScaffoldingAssembly item)
        {
            Release();
            assembly = item;
            onSelected.OnSelected(true);
        }

        public bool TryGet(out ScaffoldingAssembly item)
        {
            item = assembly;

            return item != null;
        }

        public void Release()
        {
            assembly?.RemoveHighlight();
            assembly = null;
        }
    }
}