using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.ScriptableObjects;

namespace Assets.Scripts.Shared.Containers.Collision
{
    public class CollisionInfo
    {
        public ConnectionDefinitionCollection Source { get; set; }
        public ConnectionDefinitionCollection Target { get; set; }
        public HakiComponent SourceScaffoldingComponent { get; set; }
        public HakiComponent TargetScaffoldingComponent { get; set; }
        public int SourceConnectionIndex { get; set; }
        public int TargetConnectionIndex { get; set; } = -1;

        public bool IsSuccess => IsValid();

        bool IsValueBetween(int value, int min, int max)
        {
            if (value < min)
                return false;

            if (value > max)
                return false;

            return value != max ;
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




            if (IsValueBetween(SourceConnectionIndex, 0, Source.Count) == false)
            {
                return false;
            }


            if (IsValueBetween(TargetConnectionIndex, 0, Target.Count) == false)
            {
                return false;
            }

            return true;
        }


    }
}