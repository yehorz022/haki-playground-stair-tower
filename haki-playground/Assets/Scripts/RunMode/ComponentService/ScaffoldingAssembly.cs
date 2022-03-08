using System;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Instanciation;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.ScriptableObjects;
using Assets.Scripts.Shared.Shapes;
using UnityEngine;

namespace Assets.Scripts.RunMode.ComponentService
{
    public class ScaffoldingAssembly : HakiComponent
    {

        [Inject] private IObjectCacheManager ObjectCacheManager { get; set; }
        void Start()
        {

        }


        public override void OnCache(Transform newParent)
        {
            base.OnCache(newParent);

            ScaffoldingComponent[] childrens = GetComponentsInChildren<ScaffoldingComponent>();

            foreach (ScaffoldingComponent element in childrens)
            {
                ObjectCacheManager.Cache(element);
            }
        }

        /// <inheritdoc />
        public override bool TryGetCollectionDefinition(out ConnectionDefinitionCollection collection)
        {
            collection = null;

            return false;
        }

        /// <inheritdoc />
        public override Box GetBounds()
        {
            throw new NotImplementedException("This feature has not yet been implemented!");
        }
    }
}