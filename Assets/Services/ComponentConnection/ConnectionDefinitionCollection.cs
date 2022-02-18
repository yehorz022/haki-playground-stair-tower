using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Services.ComponentConnection
{
    [CreateAssetMenu(fileName = nameof(ConnectionDefinitionCollection), menuName = "Component/Connection/Connection Definition Collection", order = 10)]
    public class ConnectionDefinitionCollection : ScriptableObject
    {
        public int Count => connectionDefinitions.Length;
        public ConnectionDefinition GetElementAt(int index)
        {
            return connectionDefinitions[index];
        }

        [SerializeField]
        internal ConnectionDefinition[] connectionDefinitions;

    }
}