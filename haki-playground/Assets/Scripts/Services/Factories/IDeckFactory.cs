using Assets.Scripts.Shared.Behaviours;
using UnityEngine;

namespace Assets.Scripts.Services.Factories
{
    public interface IDeckFactory
    {
        void Produce(Transform parent, HakiComponent prefab, int length, int width, int elementWidth, int level);
    }
}