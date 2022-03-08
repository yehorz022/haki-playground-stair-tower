using Assets.Scripts.Shared.Enums;

namespace Assets.Scripts.Services.Tools
{
    public interface IToolHandlerService
    {
        void Update();
        bool SelectToolByType(ToolType toolType);
    }
}