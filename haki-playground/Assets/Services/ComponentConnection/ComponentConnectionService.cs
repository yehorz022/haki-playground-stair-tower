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
            if (connectionDefinitionCollection == null)
                return;

            for (int i = 0; i < connectionDefinitionCollection.Count; i++)
            {

                Color color = default;

                switch (i % 4)
                {
                    case 0:
                        color = Color.red;
                        break;
                    case 1:
                        color = Color.yellow;
                        break;
                    case 2:
                        color = Color.blue;
                        break;
                    case 3:
                        color = Color.green;
                        break;
                }

                connectionDefinitionCollection.GetElementAt(i).DebugDraw(transform, color);
            }
        }
    }
}