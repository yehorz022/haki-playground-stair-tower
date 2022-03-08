using Assets.Scripts.Services.Core;
using Assets.Scripts.Shared.Constants;
using Assets.Scripts.Shared.Metrics.Metric_Units;

namespace Assets.Scripts.Services.Converters.MetricConverters
{
    [Service(typeof(IConverter<MilliMeter, Native>))]
    public class MilliMeterToNativeConverter : IConverter<MilliMeter, Native>
    {
        /// <inheritdoc />
        public Native Convert(MilliMeter input)
        {
            Native res = Constants.MilimitersToUnityFactor * input;

            return res;
        }
    }
}