using System.Collections.Generic;
using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.Services.Instanciation;
using UnityEngine;

namespace Assets.Scripts.RunMode
{
    public partial class ApplicationManager
    {
        private class ObjectCacheManager : IObjectCacheManager
        {
            private readonly IDictionary<string, Stack<GameObject>> cache = new Dictionary<string, Stack<GameObject>>();

            private readonly ApplicationManager parent;

            public ObjectCacheManager(ApplicationManager parent)
            {
                this.parent = parent;
            }

            public T Instantiate<T>(T template) where T : Component
            {
                return CreateInstance(template, parent.transform, Vector3.zero, Quaternion.identity);
            }

            public T Instantiate<T>(T template, Quaternion rotation) where T : Component
            {
                return CreateInstance(template, parent.transform, Vector3.zero, rotation);
            }

            public T Instantiate<T>(T template, Vector3 position) where T : Component
            {
                return CreateInstance(template, parent.transform, position, Quaternion.identity);
            }

            public T Instantiate<T>(T template, Vector3 position, Quaternion rotation) where T : Component
            {
                return CreateInstance(template, parent.transform, position, rotation);
            }

            public T Instantiate<T>(T template, Transform parent) where T : Component
            {
                return CreateInstance(template, parent, Vector3.zero, Quaternion.identity);
            }

            public T Instantiate<T>(T template, Transform parent, Quaternion rotation) where T : Component
            {
                return CreateInstance(template, parent, Vector3.zero, rotation);
            }

            public T Instantiate<T>(T template, Transform parent, Vector3 position, Quaternion rotation) where T : Component
            {
                return CreateInstance(template, parent, position, rotation);
            }
            public T Instantiate<T>(T template, Transform parent, Vector3 position) where T : Component
            {
                return CreateInstance(template, parent, position, Quaternion.identity);
            }


            private T UnityInstanciate<T>(T template, Transform parent) where T : Component
            {
                return GameObject.Instantiate(template, parent, false);
            }

            private static void SetData<T>(T item, Transform parent, Vector3 position, Quaternion rotation) where T : Component
            {
                if (parent)
                    item.transform.SetParent(parent);
                if (position != Vector3.zero)
                    item.transform.localPosition = position;
                if (rotation != Quaternion.identity)
                    item.transform.localRotation = rotation;
            }

            private T GetInstanceOf<T>(T template, Transform parent) where T : Component
            {
                if (cache.TryGetValue(template.name, out Stack<GameObject> stack) is false || stack.Count == 0)
                {
                    return UnityInstanciate(template, parent);
                }
                else
                {
                    GameObject go = stack.Pop();
                    go.SetActive(true);
                    return go.GetComponent<T>();
                }
            }

            public T CreateInstance<T>(T template, Transform parent, Vector3 position, Quaternion rotation) where T : Component
            {

                T res = GetInstanceOf(template, parent);

                SetData(res, parent, position, rotation);
                HandleDependencyInjection(res);

                return res;

            }

            private void HandleDependencyInjection<T>(T item) where T : Component
            {
                ApplicationManager.HandleDependencyInjection(item as HakiComponent);
            }

            public void Cache<T>(T item) where T : Component
            {
                item.name = item.name.Replace("(Clone)", "").Trim();

                if (cache.TryGetValue(item.name, out Stack<GameObject> stack) is false)
                {
                    stack = new Stack<GameObject>();
                    cache.Add(item.name, stack);
                }

                item.transform.SetParent(parent.transform);
                item.transform.position = Vector3.zero;
                item.transform.rotation = Quaternion.identity;
                item.gameObject.SetActive(false);

                stack.Push(item.gameObject);
            }
        }

    }
}