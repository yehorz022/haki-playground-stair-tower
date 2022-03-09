using System.Collections.Generic;
using Assets.Scripts.Shared.Behaviours;
using UnityEngine;

namespace Assets.Scripts.Services.Tools.Selector.Face
{
    public interface IBoxFinder<T> : IFinder<IList<T>> where T : HakiComponent
    {
        void SetBox(Vector3 position, Vector3 size);
    }

    public interface IFinder<T> 
    {
        bool TryFind(out T item);
    }
    public interface ISelectFaceTool : ITool, IFinder<HakiComponent>
    {
    }

    public interface ISelected<T>
    {

        void SetSelected(T item);
        bool TryGet(out T item);
        void Release();
    }

    public interface IOnSelected
    {
        void OnSelected(bool value);
    }
}