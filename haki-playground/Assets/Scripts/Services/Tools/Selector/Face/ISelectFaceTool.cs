using Assets.Scripts.Shared.Behaviours;

namespace Assets.Scripts.Services.Tools.Selector.Face
{
    public interface ISelector<T>
    {
        bool TrySelect(out T item);
    }
    public interface ISelectFaceTool : ITool, ISelector<HakiComponent>
    {
    }
}