using System.Collections.Generic;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Tools.Selector.Face;
using Assets.Scripts.Shared.Enums;

namespace Assets.Scripts.Services.Tools
{
    [Service(typeof(IToolHandlerService))]
    public class ToolHandler : IToolHandlerService
    {
        private readonly IDictionary<ToolType, ITool> tools;
        private ToolType currentKey = ToolType.None;

        private ITool SelectedTool => tools.TryGetValue(currentKey, out ITool tool) ? tool : default;
        public bool HasToolSelected => currentKey != ToolType.None && tools.ContainsKey(currentKey);

        public ToolHandler(ISelectFaceTool selectFceTool)
        {
            tools = new Dictionary<ToolType, ITool>();
            tools.Add(selectFceTool.ToolType, selectFceTool);
        }

        public void Update()
        {
            SelectedTool?.Update();
        }

        /// <inheritdoc />
        public bool SelectToolByType(ToolType toolType)
        {
            if (toolType == ToolType.None || tools.ContainsKey(toolType) == false)
                return false;

            currentKey = toolType;
            return toolType == currentKey;
        }
    }
}