using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.Shared.Behaviours;

namespace Assets.Scripts.RunMode.ComponentConnection
{
    public class AssemblyComponentLocalizationInfo
    {
        public ScaffoldingComponent ScaffoldingComponent { get; set; }
        public int OutputConnectionIndex { get; set; }
        public int InputConnectionIndex { get; set; }
    }
}