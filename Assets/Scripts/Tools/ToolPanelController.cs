using UnityEngine;

namespace Assets.Scripts.Tools
{
  public class ToolPanelController : MonoBehaviour, IToolChangeObserver
  {
    public TextMesh toolgunModeText;
    public Renderer panel;

    public void OnToolsChanged(ITool newTool) => SetToolgunModeText(newTool.ToolName);

    public void SetBackgroundColor(Color color) => panel.material.color = color;

    public void SetToolgunModeText(string text) => toolgunModeText.text = text;
  }
}
