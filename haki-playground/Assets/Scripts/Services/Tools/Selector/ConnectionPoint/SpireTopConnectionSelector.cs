using Assets.Scripts.Services.Core;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Constants;
using Assets.Scripts.Shared.ScriptableObjects;

namespace Assets.Scripts.Services.Tools.Selector.ConnectionPoint
{
    [Service(typeof(ISpireTopConnectionSelector))]
    public class SpireTopConnectionSelector : IndexBasedConnectionSelector, ISpireTopConnectionSelector
    {
        /// <inheritdoc />

        public override bool GetConnection(HakiComponent component, out ConnectionDefinition definition)
        {
            return GetConnectionByIndex(component, out definition, Constants.IndexOfTopConnectionPointInStandardSpire);
        }
    }
}