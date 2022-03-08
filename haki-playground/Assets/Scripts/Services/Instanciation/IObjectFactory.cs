using Assets.Scripts.Shared.Behaviours;
using UnityEngine;

namespace Assets.Scripts.Services.Instanciation
{
    public interface IObjectCacheManager
    {
        void Cache<T>(T item) where T : HakiComponent;
        T Instantiate<T>(T template) where T : HakiComponent;

        T Instantiate<T>(T template, Quaternion rotation) where T : HakiComponent;
        T Instantiate<T>(T template, Transform parent) where T : HakiComponent;
        T Instantiate<T>(T template, Transform parent, Quaternion rotation) where T : HakiComponent;
        T Instantiate<T>(T template, Transform parent, Vector3 position) where T : HakiComponent;
        T Instantiate<T>(T template, Transform parent, Vector3 position, Quaternion rotation) where T : HakiComponent;
        T Instantiate<T>(T template, Vector3 position) where T : HakiComponent;
        T Instantiate<T>(T template, Vector3 position, Quaternion rotation) where T : HakiComponent;
    }
}