using Assets.Scripts.Services.Core;
using Assets.Scripts.Shared.Constants;

namespace Assets.Scripts.Services.Converters
{
    public interface IMillimitersToUnityUnitConverter : IConverter<int, float>
    {
    }

    public interface ICentiMetersToUnityConverter : IConverter<int, float>
    {

    }

    [Service(typeof(ICentiMetersToUnityConverter))]
    public class CentimetersToUnityConverter : ICentiMetersToUnityConverter
    {
        public float Convert(int input)
        {
            return Constants.CentiMetersToUnityFactor * input;
        }
    }

    [Service(typeof(IMillimitersToUnityUnitConverter))]
    public class MillimetersToUnityConverter : IMillimitersToUnityUnitConverter
    {
        public float Convert(int input)
        {
            return Constants.MilimitersToUnityFactor * input;
        }
    }
}