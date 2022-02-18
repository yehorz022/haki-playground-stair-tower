using Assets.Services.ComponentService;
using UnityEngine;

namespace Assets.Services.ComponentConnection
{
    public class ComponentConnectionService : HakiComponent
    {
        [SerializeField]
        internal ConnectionDefinitionCollection connectionDefinitionCollection;


        protected override void DebugDraw(bool isSelected)
        {
            for (int i = 0; i < connectionDefinitionCollection.Count; i++)
            {
                connectionDefinitionCollection.GetElementAt(i).DebugDraw(transform);
            }
        }
    }
}