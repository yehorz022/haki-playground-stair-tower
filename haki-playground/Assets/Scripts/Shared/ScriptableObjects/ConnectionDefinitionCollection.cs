using UnityEngine;

namespace Assets.Scripts.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(ConnectionDefinitionCollection), menuName = "ScaffoldingComponent/Connection/Connection Definition Collection", order = 10)]
    public class ConnectionDefinitionCollection : ScriptableObject
    {
        public int Count => connectionDefinitions?.Length ?? 0;

        public ConnectionDefinition GetElementAt(int index)
        { // this is done this way instead of indexer because Unity doesnt like when Scriptable objecs have indexers
            return connectionDefinitions[index];
        }

        [SerializeField]
        public ConnectionDefinition[] connectionDefinitions;

    }
}