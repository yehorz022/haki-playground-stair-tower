using Assets.Scripts.Services.ComponentService;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Instanciation;
using Assets.Scripts.Shared.Behaviours;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services.Storage
{
    [Service(typeof(IProject))]
    public class Project : IProject
    {
        private int id;
        private readonly IComponentHolder componentHolder;
        private readonly IObjectCacheManager objectCacheManager;

        public Project(int id, IComponentHolder componentHolder, IObjectCacheManager objectCacheManager)
        {
            this.id = id;
            this.componentHolder = componentHolder;
            this.objectCacheManager = objectCacheManager;
        }

        public void Load(Transform parent, List<HakiComponent>components)
        {
            //Debug.Log("ComponentsCount = " + components.Count);
            int count = PlayerPrefs.GetInt("Project" + id + "ComponentsCount", 0);
            for (int i = 0; i < count; i++)
            {
                int componentId = PlayerPrefs.GetInt("project" + id + "component" + i + "id");
                //Debug.Log("Component " + i + " id = " + id + " name " + (GetComponentByID(components, componentId) != null ? GetComponentByID(components, componentId).name : "null"));
                HakiComponent res = Produce(GetComponentByID(components, componentId), parent);
                res.Read(id, i);
            }
        }

        public void Save()
        {
            List<HakiComponent> components = componentHolder.GetAllComponents();
            PlayerPrefs.SetInt("Project" + id + "ComponentsCount", components.Count);
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Write(id, i);
            }
        }

        protected T Produce<T>(T prefab, Transform parent) where T : HakiComponent
        {
            T res = objectCacheManager.Instantiate(prefab, parent);
            componentHolder.PlaceComponent(res);
            return res;
        }

        public HakiComponent GetComponentByID(List<HakiComponent> components, int componentId)
        {
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].id == componentId)
                    return components[i];
            }
            return null;
        }
    }
}