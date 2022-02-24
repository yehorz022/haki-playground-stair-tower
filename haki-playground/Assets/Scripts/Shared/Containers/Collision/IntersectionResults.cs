using Assets.Scripts.Shared.Interfaces;
using Assets.Scripts.Shared.ScriptableObjects;

namespace Assets.Scripts.Shared.Containers.Collision
{
    public class IntersectionResults
    {

        public ConnectionDefinitionCollection ConnectionDefinitionCollection { get; }
        public int ConnectionIndex { get; }
        public IScaffoldingComponent ScaffoldingComponent { get; }

        public IntersectionResults(IScaffoldingComponent scaffoldingComponent, int connectionIndex)
        {
            ScaffoldingComponent = scaffoldingComponent;
            ConnectionDefinitionCollection = scaffoldingComponent.GetConnectionDefinitionCollection();
            ConnectionIndex = connectionIndex;
        }
    }
}