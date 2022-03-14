using System;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Shared.Metrics.Metric_Units;

namespace Assets.Scripts.Services.Converters.MetricConverters
{
    [Service(typeof(IConverter<Native, CentiMeter>))]
    public class NativeToCentiMetersConverter : IConverter<Native, CentiMeter>
    {
        /// <inheritdoc />
        public CentiMeter Convert(Native input)
        {
            throw new NotImplementedException("NOT IMPLEMENTED");
            //return Constants.MilimitersToUnityFactor / input;
        }
    }
}