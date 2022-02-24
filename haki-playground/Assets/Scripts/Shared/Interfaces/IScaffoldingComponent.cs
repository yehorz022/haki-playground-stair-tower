using Assets.Scripts.Shared.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Shared.Interfaces
{
    public interface IScaffoldingComponent
    {
        Transform GetTransform();
        GameObject GetGameObject();
        ConnectionDefinitionCollection GetConnectionDefinitionCollection();
    }
}
