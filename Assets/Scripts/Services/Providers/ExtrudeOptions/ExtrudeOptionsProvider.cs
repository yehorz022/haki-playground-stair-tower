using Assets.Scripts.Services.Core;
using Assets.Scripts.Shared.Enums;


namespace Assets.Scripts.Services.Providers.ExtrudeOptions
{
    [Service(typeof(IExtrudeOptionsProvider))]
    public class ExtrudeOptionsProvider : IExtrudeOptionsProvider
    {
        private ExtrudeTechnique technique;
        public ExtrudeTechnique GetOptions()
        {
            return technique;
        }

        public bool SetOptions(ExtrudeTechnique technique)
        {
            bool res = technique != this.technique;
            this.technique = technique;
            return res;
        }
    }
}