using Assets.Scripts.Shared.Behaviours;
using UnityEngine;

namespace Assets.Scripts.Services.Factories
{
    public interface ISidesFactory
    {
        public void Produce(Transform parent, HakiComponent prefabAlongZ, HakiComponent prefabAlongX, int length, int width, int level);
    }
}