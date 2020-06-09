using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Canvas
{
  public class ContextMenuHandler : MonoBehaviour
  {
    private Node _node;

    public UnityEngine.GameObject contextMenu;
    private GraphService _graphService;

    public void Start()
    {
      _graphService = FindObjectOfType<GraphService>();
      contextMenu.SetActive(false);
    }

    public void OpenContextMenu(UnityEngine.GameObject gameObjectHit)
    {
      var node = _graphService.FindNodeByGameObject(gameObjectHit);
      _node = node;
      contextMenu.SetActive(true);
      GameService.Instance.PauseGame();
    }

    public void ChangeParametersButtonOnClick()
    {
      FindObjectOfType<PropertiesMenuHandler>().OpenPropertiesMenu(_node);
    }

    public void DeleteButtonOnClick()
    {
      _graphService.RemoveNode(_node);
      ExitButtonOnClick();
    }

    public void ExitButtonOnClick()
    {
      contextMenu.SetActive(false);
      GameService.Instance.UnPauseGame();
    }
  }
}
