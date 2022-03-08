using Assets.Scripts.Services.Core;
using Assets.Scripts.Shared.Constants;

namespace Assets.Scripts.Services.Converters.MetricConverters
{
    [Service(typeof(IMillimitersToUnityUnitConverter))]
    public class MillimetersToUnityConverter : IMillimitersToUnityUnitConverter
    {
        public float Convert(int input)
        {
            return Constants.MilimitersToUnityFactor * input;
        }
    }
}