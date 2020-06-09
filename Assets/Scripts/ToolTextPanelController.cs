using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts
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
