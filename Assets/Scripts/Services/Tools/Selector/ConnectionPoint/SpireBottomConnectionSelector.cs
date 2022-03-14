using Assets.Scripts.Services.Core;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Constants;
using Assets.Scripts.Shared.ScriptableObjects;

namespace Assets.Scripts.Services.Tools.Selector.ConnectionPoint
{
    [Service(typeof(ISpireBottomConnectionSelector))]
    public class SpireBottomConnectionSelector : IndexBasedConnectionSelector, ISpireBottomConnectionSelector
    {
        /// <inheritdoc />
        public override bool GetConnection(HakiComponent component, out ConnectionDefinition definition)
        {
            return GetConnectionByIndex(component, out definition, Constants.IndexOfBottomConnectionPointInStandardSpire);
        }
    }
}