using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Tools.Selector.Face;

namespace Assets.Scripts.RunMode
{
    [Service(typeof(ISelected<ScaffoldingAssembly>))]
    public class SelectedAssembly : ISelected<ScaffoldingAssembly>
    {
        private ScaffoldingAssembly assembly;

        public void SetSelected(ScaffoldingAssembly item)
        {
            Release();
            assembly = item;
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