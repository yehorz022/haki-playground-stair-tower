using System.Linq;
using UnityEngine;

namespace Assets.Services.ComponentConnection
{
    [CreateAssetMenu(fileName = nameof(ConnectionDefinitionCollection), menuName = "Component/Connection/Connection Definition Collection", order = 10)]
    public class ConnectionDefinitionCollection : ScriptableObject
    {
        public int Count => connectionDefinitions?.Length ?? 0;

        public ConnectionDefinition MainConnection => connectionDefinitions?.FirstOrDefault();

        public ConnectionDefinition GetElementAt(int index)
        {
            return connectionDefinitions[index];
        }

        [SerializeField]
        internal ConnectionDefinition[] connectionDefinitions;

    }
}