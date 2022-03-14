namespace Assets.Scripts.Services.Providers.ExtrudeOptions
{
    public interface IExtrudeOptionsProvider
    {
        ExtrudeTechnique GetOptions();
        bool SetOptions(ExtrudeTechnique technique);
    }
}