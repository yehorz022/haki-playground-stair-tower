
namespace Assets.Scripts.Shared.Constants
{
    public static class Constants
    {
        public const int IndexOfTopConnectionPointInStandardSpire = 1;
        public const int IndexOfBottomConnectionPointInStandardSpire = 0;
        public const float VirtualSphereRadius = .15f;
        public const string ImproperParameters = "Improper parameters";
        public const string TooManyConstructorsException = "Services can have only one constructor empty constructor or one constructor with parameters";
        public const string ConnectionDefinitionsIsEmpty = "Connection definition is empty";
        public const float MinimumDotProductValueAllowedForIntersections = 1f;
        public const float MilimitersToUnityFactor = 0.001f;
        public const float CentiMetersToUnityFactor = 0.01f;
        public const string ReflectionServiceFactoryIsNull = "ReflectionServiceFactory.constructor is null";
        public const string ReflectionServiceFactoryTypesIsNull = "ReflectionServiceFactory.types is null";
        public const int SpireInitialOffset = 240;
        public const int SpireSpacingBetweenPocketGroups = 500;
        public const int DeckVerticalOffset = 45;

        public const string CloneGameObjectSuffix = "(Clone)";
        public const string cacheSuffix = " - cache";
    }
}
