using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.ScriptableObjects;

namespace Assets.Scripts.Shared.Containers.Collision
{
    public class IntersectionResults
    {

        public ConnectionDefinitionCollection ConnectionDefinitionCollection { get; }
        public int ConnectionIndex { get; }
        public HakiComponent ScaffoldingComponent { get; }

        public IntersectionResults(HakiComponent scaffoldingComponent, int connectionIndex, ConnectionDefinitionCollection connectionDefinitionCollection)
        {
            ScaffoldingComponent = scaffoldingComponent;
            ConnectionDefinitionCollection = connectionDefinitionCollection;
            ConnectionIndex = connectionIndex;
        }
    }
}