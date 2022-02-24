using Assets.Scripts.Services.Core;
using UnityEngine;

namespace Assets.Scripts.Services.Instanciation
{
    public interface IObjectCacheManager
    {
        void Cache<T>(T item) where T : Component;
        T CreateInstance<T>(T template, Transform parent, Vector3 position, Quaternion rotation) where T : Component;
        T Instantiate<T>(T template) where T : Component;
        T Instantiate<T>(T template, Quaternion rotation) where T : Component;
        T Instantiate<T>(T template, Transform parent) where T : Component;
        T Instantiate<T>(T template, Transform parent, Quaternion rotation) where T : Component;
        T Instantiate<T>(T template, Transform parent, Vector3 position) where T : Component;
        T Instantiate<T>(T template, Transform parent, Vector3 position, Quaternion rotation) where T : Component;
        T Instantiate<T>(T template, Vector3 position) where T : Component;
        T Instantiate<T>(T template, Vector3 position, Quaternion rotation) where T : Component;
    }


}