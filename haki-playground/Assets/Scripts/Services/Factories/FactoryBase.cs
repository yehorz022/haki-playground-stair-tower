using Assets.Scripts.Services.ComponentService;
using Assets.Scripts.Services.Instanciation;
using Assets.Scripts.Shared.Behaviours;
using UnityEngine;

namespace Assets.Scripts.Services.Factories
{
    public abstract class FactoryBase
    {
        private readonly IComponentHolder componentHolder;
        private readonly IObjectCacheManager objectCacheManager;

        protected FactoryBase(IComponentHolder componentHolder, IObjectCacheManager objectCacheManager)
        {
            this.componentHolder = componentHolder;
            this.objectCacheManager = objectCacheManager;
        }

        protected T Produce<T>(T prefab, Transform parent) where T : HakiComponent
        {
            T res = objectCacheManager.Instantiate(prefab, parent);
            componentHolder.PlaceComponent(res);
            return res;
        }

    }
}