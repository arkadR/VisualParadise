using UnityEngine;

namespace Assets.Scripts.Tools
{
  public class ToolTextPanelController : MonoBehaviour, IToolChangeObserver
  {
    public TextMesh gunModeText;

    public void OnToolsChanged(ITool newTool)
    {
      gunModeText.text = newTool.ToolName;
    }
  }
}
