using UnityEngine;

namespace Assets.Scripts.Tools
{
  public class ToolPanelController : MonoBehaviour, IToolChangeObserver
  {
    public TextMesh toolgunModeName;
    public TextMesh toolgunModeInfo;
    public Renderer panel;

    public void OnToolsChanged(ITool newTool)
    {
      toolgunModeName.text = newTool.ToolName;
      SetToolgunModeInfoText("");
    }

    public void SetBackgroundColor(Color color) => panel.material.color = color;

    public void SetToolgunModeInfoText(string text) => toolgunModeInfo.text = text;
  }
}
