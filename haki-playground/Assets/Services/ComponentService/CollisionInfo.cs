using Assets.Services.ComponentConnection;

namespace Assets.Services.ComponentService
{
    public class CollisionInfo
    {
        public ConnectionDefinitionCollection Source { get; set; }
        public ConnectionDefinitionCollection Target { get; set; }
        public ScaffoldingComponent SourceScaffoldingComponent { get; set; }
        public ScaffoldingComponent TargetScaffoldingComponent { get; set; }
        public int SourceConnectionIndex { get; set; }
        public int TargetConnectionIndex { get; set; } = -1;

        public bool IsSuccess => IsValid();


        internal ConnectionDefinition GetSourceConnectionDefinition()
        {
            return Source.GetElementAt(SourceConnectionIndex);
        }

        internal ConnectionDefinition GetTargetConnectionDefinition()
        {
            return Target.GetElementAt(SourceConnectionIndex);
        }

        private bool IsValid()
        {
            if (Target == null)
                return false;
            if (Source == null)
                return false;

            if (SourceScaffoldingComponent == null)
                return false;
            if (TargetScaffoldingComponent == null)
                return false;

            if (SourceConnectionIndex.IsValueBetween(0, Source.Count) == false)
                return false;
            if (TargetConnectionIndex.IsValueBetween(0, Target.Count) == false)
                return false;

            return true;
        }


    }
}