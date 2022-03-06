using Assets.Scripts.Services.Core;
using Assets.Scripts.Shared.Constants;
using Assets.Scripts.Shared.Metrics.Metric_Units;

namespace Assets.Scripts.Services.Converters
{
    [Service(typeof(IConverter<CentiMeter, Native>))]
    public class CentiMeterToNativeConverter : IConverter<CentiMeter, Native>
    {
        /// <inheritdoc />
        public Native Convert(CentiMeter input)
        {
            return Constants.CentiMetersToUnityFactor * input;
        }
    }
}