using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Services.Tools.Selector.ConnectionPoint
{
    public abstract class IndexBasedConnectionSelector : IConnectionPointSelector
    {
        /// <inheritdoc />
        public abstract bool GetConnection(HakiComponent component, out ConnectionDefinition definition);


        protected bool GetConnectionByIndex(HakiComponent component, out ConnectionDefinition definition, int index)
        {
            if (component.TryGetCollectionDefinition(out ConnectionDefinitionCollection collection))
            {
                definition = collection.GetElementAt(index);
                return true;
            }


            Debug.LogError($"Connection definitions of {component.name} at index {index} was not found!");
            definition = default;
            return false;
        }
    }
}