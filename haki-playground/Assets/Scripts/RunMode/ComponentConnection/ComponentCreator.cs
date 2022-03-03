using System;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.RunMode.ComponentConnection
{
    public class ComponentCreator : MonoBehaviour
    {
        [NonSerialized]
        public Transform[] connectors;
        [NonSerialized]
        public ConnectionTypeA[] componentType;

        public Transform reference;
        public ConnectionDefinition look;

        [Range(0, 10)]
        public int count;


        private int oldCount = -1;

        public void HandleCount()
        {
            if (oldCount == count && connectors != null && componentType != null)
                return;

            if (connectors == null)
            {
                connectors = new Transform[count];
                componentType = new ConnectionTypeA[count];
            }
            else
            {
                Transform[] tConnectors = new Transform[count];
                ConnectionTypeA[] tComponentTypes = new ConnectionTypeA[count];
                for (int i = 0; i < connectors.Length && i < count; i++)
                {
                    tConnectors[i] = connectors[i];
                    if (componentType != null) 
                        tComponentTypes[i] = componentType[i];
                }

                connectors = tConnectors;
                componentType = tComponentTypes;
            }

            oldCount = count;
        }

        [SerializeField]
        public int Count
        {
            get => oldCount;
        }
    }
}