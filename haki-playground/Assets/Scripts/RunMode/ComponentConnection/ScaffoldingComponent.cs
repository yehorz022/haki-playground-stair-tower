using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.Shared.Interfaces;
using Assets.Scripts.Shared.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.RunMode.ComponentConnection
{
    public class ScaffoldingComponent : HakiComponent, IScaffoldingComponent
    {
        public ConnectionDefinitionCollection ConnectionDefinitionCollection;
        public Sprite icon;

        protected override void DebugDraw(bool isSelected)
        {
            if (ConnectionDefinitionCollection == null)
                return;

            for (int i = 0; i < ConnectionDefinitionCollection.Count; i++)
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

                ConnectionDefinitionCollection.GetElementAt(i).DebugDraw(transform, color);
            }
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public ConnectionDefinitionCollection GetConnectionDefinitionCollection()
        {
            return ConnectionDefinitionCollection;
        }
    }
}