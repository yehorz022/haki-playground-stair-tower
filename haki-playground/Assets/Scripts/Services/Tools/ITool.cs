using Assets.Scripts.Shared.Enums;

namespace Assets.Scripts.Services.Tools
{
    public interface ITool
    {
        ToolType ToolType { get; }
        void Update();
    }
}