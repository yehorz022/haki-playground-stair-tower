using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Services.ComponentService
{
    public class ObjectCacheManager : MonoBehaviour
    {
        private readonly IDictionary<string, Stack<GameObject>> _cache = new Dictionary<string, Stack<GameObject>>();
        private DependancyInjectionManager dim;

        [Inject]
        public IIntersectionService IntersectionService { get; set; }

        public DependancyInjectionManager DependancyInjectionManager
        {
            get
            {
                if (dim == null)
                    dim = FindObjectOfType<DependancyInjectionManager>();
                return dim;
            }
        }

        public new T Instantiate<T>(T template) where T : Component
        {
            return CreateInstance(template, null, Vector3.zero, Quaternion.identity);
        }

        public T Instantiate<T>(T template, Quaternion rotation) where T : Component
        {
            return CreateInstance(template, null, Vector3.zero, rotation);
        }

        public T Instantiate<T>(T template, Vector3 position) where T : Component
        {
            return CreateInstance(template, null, position, Quaternion.identity);
        }

        public new T Instantiate<T>(T template, Vector3 position, Quaternion rotation) where T : Component
        {
            return CreateInstance(template, null, position, rotation);
        }

        public new T Instantiate<T>(T template, Transform parent) where T : Component
        {
            return CreateInstance(template, parent, Vector3.zero, Quaternion.identity);
        }

        public T Instantiate<T>(T template, Transform parent, Quaternion rotation) where T : Component
        {
            return CreateInstance(template, parent, Vector3.zero, rotation);
        }

        public T Instantiate<T>(T template, Transform parent, Vector3 position, Quaternion rotation) where T : Component
        {
            return CreateInstance(template, transform, position, rotation);
        }
        public T Instantiate<T>(T template, Transform parent, Vector3 position) where T : Component
        {
            return CreateInstance(template, parent, position, Quaternion.identity);
        }


        private T UnityInstanciate<T>(T template, Transform parent) where T : Component
        {
            return Instantiate(template, parent, false);
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
            if (_cache.TryGetValue(template.name, out Stack<GameObject> stack) is false || stack.Count == 0)
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
            DependancyInjectionManager.InjectDependencies(item);
        }

        public void Cache<T>(T item) where T : Component
        {
            item.name = item.name.Replace("(Clone)", "").Trim();

            if (_cache.TryGetValue(item.name, out Stack<GameObject> stack) is false)
            {
                stack = new Stack<GameObject>();
                _cache.Add(item.name, stack);
            }

            item.transform.SetParent(transform);
            item.transform.position = Vector3.zero;
            item.transform.rotation = Quaternion.identity;
            item.gameObject.SetActive(false);

            stack.Push(item.gameObject);
        }
    }
}