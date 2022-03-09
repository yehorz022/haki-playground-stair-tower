using System;
using System.Collections.Generic;
using Assets.Scripts.Services.DependencyInjection;
using Assets.Scripts.Services.Instanciation;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Constants;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.RunMode
{
    public partial class ApplicationManager
    {
        private class DependencyInjection : IDependencyInjection
        {
            private readonly Action<SceneMemberInjectDependencies> injectionDelegate;

            public DependencyInjection(Action<SceneMemberInjectDependencies> injectionDelegate)
            {
                this.injectionDelegate = injectionDelegate;
            }

            /// <inheritdoc />
            public void HandleInjections(SceneMemberInjectDependencies item)
            {
                if (item == null)
                {
                    return;
                }

                if (item.GetDependencyInjectionStatus())
                {
                    return;
                }

                injectionDelegate.Invoke(item);

                item.FinalizeDependancyInjection();
            }
        }

        private class ObjectCacheManager : IObjectCacheManager
        {
            private readonly IDictionary<string, Stack<HakiComponent>> cache = new Dictionary<string, Stack<HakiComponent>>();

            private readonly Transform applicationManagerTransform;
            private readonly GameObject emptyPrefab;
            private readonly IDependencyInjection dependencyInjectionHandler;
            private readonly IDictionary<string, Transform> parents;

            public ObjectCacheManager(Component parent, GameObject emptyPrefab, IDependencyInjection dependencyInjection)
            {
                applicationManagerTransform = parent.transform;
                this.emptyPrefab = emptyPrefab;
                parents = new Dictionary<string, Transform>();
                dependencyInjectionHandler = dependencyInjection;
            }

            private Transform GetParent(string itemName)
            {
                if (parents.TryGetValue(itemName, out Transform res) == false)
                {
                    res = UnityInstanciate(emptyPrefab, applicationManagerTransform).transform;
                    res.name = itemName + Constants.cacheSuffix;
                    parents.Add(itemName, res);
                }

                return res;
            }

            public T Instantiate<T>(T template) where T : HakiComponent
            {
                return CreateInstance(template, applicationManagerTransform.transform, Vector3.zero, Quaternion.identity);
            }

            public T Instantiate<T>(T template, Quaternion rotation) where T : HakiComponent
            {
                return CreateInstance(template, applicationManagerTransform.transform, Vector3.zero, rotation);
            }

            public T Instantiate<T>(T template, Vector3 position) where T : HakiComponent
            {
                return CreateInstance(template, applicationManagerTransform.transform, position, Quaternion.identity);
            }

            public T Instantiate<T>(T template, Vector3 position, Quaternion rotation) where T : HakiComponent
            {
                return CreateInstance(template, applicationManagerTransform.transform, position, rotation);
            }

            public T Instantiate<T>(T template, Transform parent) where T : HakiComponent
            {
                return CreateInstance(template, parent, Vector3.zero, Quaternion.identity);
            }

            public T Instantiate<T>(T template, Transform parent, Quaternion rotation) where T : HakiComponent
            {
                return CreateInstance(template, parent, Vector3.zero, rotation);
            }

            public T Instantiate<T>(T template, Transform parent, Vector3 position, Quaternion rotation) where T : HakiComponent
            {
                return CreateInstance(template, parent, position, rotation);
            }
            public T Instantiate<T>(T template, Transform parent, Vector3 position) where T : HakiComponent
            {
                return CreateInstance(template, parent, position, Quaternion.identity);
            }


            private T UnityInstanciate<T>(T template, Transform parent) where T : Object
            {
                return Object.Instantiate(template, parent, false);
            }

            private static void SetData<T>(T item, Transform parent, Vector3 position, Quaternion rotation) where T : HakiComponent
            {
                if (parent)
                    item.transform.SetParent(parent);
                if (position != Vector3.zero)
                    item.transform.localPosition = position;
                if (rotation != Quaternion.identity)
                    item.transform.localRotation = rotation;
            }

            private T GetInstanceOf<T>(T template, Transform newParent) where T : HakiComponent
            {
                if (cache.TryGetValue(template.name, out Stack<HakiComponent> stack) is false || stack.Count == 0)
                {
                    return UnityInstanciate(template, newParent);
                }
                else
                {
                    HakiComponent go = stack.Pop();
                    go.OnInitialize(newParent);

                    return go as T;
                }
            }

            private T CreateInstance<T>(T template, Transform parent, Vector3 position, Quaternion rotation) where T : HakiComponent
            {
                T res = GetInstanceOf(template, parent);

                SetData(res, parent, position, rotation);

                dependencyInjectionHandler.HandleInjections(res);

                return res;
            }

            public void Cache<T>(T item) where T : HakiComponent
            {

                Destroy(item.gameObject); // destrying items for now since we're makign a dirty fix here and this only causes problems

                return;
                item.name = item.name.Replace(Constants.CloneGameObjectSuffix, string.Empty).Trim();

                Transform tempParent = GetParent(item.name);

                if (cache.TryGetValue(item.name, out Stack<HakiComponent> stack) is false)
                {
                    stack = new Stack<HakiComponent>();
                    cache.Add(item.name, stack);
                }

                item.OnCache(tempParent);

                stack.Push(item);
            }
        }
    }
}