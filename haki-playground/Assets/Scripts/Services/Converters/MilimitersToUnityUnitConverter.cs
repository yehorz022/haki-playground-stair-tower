using Assets.Scripts.Services.Core;
using Assets.Scripts.Shared.Constants;

namespace Assets.Scripts.Services.Converters
{
    public interface IMilimitersToUnityUnitConverter
    {
        float Convert(int input);
    }

    [Service(typeof(IMilimitersToUnityUnitConverter))]
    public class MilimitersToUnityUnitConverter : IConverter<int, float>, IMilimitersToUnityUnitConverter
    {
        public float Convert(int input)
        {
            return Constants.MilimitersToUnityFactor * input;
        }
    }
}