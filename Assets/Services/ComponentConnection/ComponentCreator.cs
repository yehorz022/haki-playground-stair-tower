using UnityEngine;

namespace Assets.Services.ComponentConnection
{
    public enum ComponentType
    {
        Pillar,
        Foot,
        SideBarrier
    }

    public class ComponentCreator : MonoBehaviour
    {
        public Transform connectors;
        public ComponentType componentType;
    }
}