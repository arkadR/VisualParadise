using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Canvas
{
  public class EdgeMenuHandler : MonoBehaviour
  {
    private Edge _edge;

    private GraphService _graphService;

    public GameObject edgeMenu;

    public void Start()
    {
      _graphService = FindObjectOfType<GraphService>();
      edgeMenu.SetActive(false);
    }

    public void OpenContextMenu(Edge edge)
    {
      _edge = edge;
      edgeMenu.SetActive(true);
      GameService.Instance.PauseGameWithoutResume();
    }

    public void ChangeParametersButtonOnClick()
    {
      FindObjectOfType<LabelMenuHandler>().OpenLabelMenu(_edge, edgeMenu);
    }

    public void DeleteButtonOnClick()
    {
      _graphService.RemoveEdge(_edge);
      ExitButtonOnClick();
    }

    public void ExitButtonOnClick()
    {
      edgeMenu.SetActive(false);
      GameService.Instance.UnPauseGameWithoutResume();
    }
  }
}
