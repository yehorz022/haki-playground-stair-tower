using Assets.Scripts.Services.Core;
using Assets.Scripts.Shared.Constants;

namespace Assets.Scripts.Services.Converters.MetricConverters
{
    [Service(typeof(ICentiMetersToUnityConverter))]
    public class CentimetersToUnityConverter : ICentiMetersToUnityConverter
    {
        public float Convert(int input)
        {
            return Constants.CentiMetersToUnityFactor * input;
        }
    }
}