using UnityEngine;

namespace Assets.Scripts.Tools
{
  public class ToolPanelController : MonoBehaviour, IToolChangeObserver
  {
    public TextMesh gunModeText;
    public Renderer panel;

    public void OnToolsChanged(ITool newTool) => gunModeText.text = newTool.ToolName;

    public void SetBackgroundColor(Color color) => panel.material.color = color;
  }
}
