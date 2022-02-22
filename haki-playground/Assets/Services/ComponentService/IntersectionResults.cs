using Assets.Services.ComponentConnection;

namespace Assets.Services.ComponentService
{
    public class IntersectionResults
    {

        public ConnectionDefinitionCollection ConnectionDefinitionCollection { get; }
        public int ConnectionIndex { get; }
        public ScaffoldingComponent ScaffoldingComponent { get; }

        public IntersectionResults(ScaffoldingComponent scaffoldingComponent, int connectionIndex)
        {
            ScaffoldingComponent = scaffoldingComponent;
            ConnectionDefinitionCollection = scaffoldingComponent.ConnectionDefinitionCollection;
            ConnectionIndex = connectionIndex;
        }
    }
}