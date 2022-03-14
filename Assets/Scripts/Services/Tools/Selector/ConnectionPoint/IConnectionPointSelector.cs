using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.ScriptableObjects;

namespace Assets.Scripts.Services.Tools.Selector.ConnectionPoint
{
    public interface IConnectionPointSelector
    {
        bool GetConnection(HakiComponent component, out ConnectionDefinition definition);
    }
}