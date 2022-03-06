using Assets.Scripts.Services.Core;
using Assets.Scripts.Shared.Constants;
using Assets.Scripts.Shared.Metrics.Metric_Units;

namespace Assets.Scripts.Services.Converters
{

    [Service(typeof(IConverter<Native, MilliMeter>))]
    public class NatveToMilliMetersConverter : IConverter<Native, MilliMeter>
    {
        /// <inheritdoc />
        public MilliMeter Convert(Native input)
        {
            return Constants.MilimitersToUnityFactor / input;
        }
    }
}