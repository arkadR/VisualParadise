using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Canvas
{
  public class EdgeMenuHandler : MonoBehaviour
  {
    private Edge _edge;

    private GraphService _graphService;

    public UnityEngine.GameObject edgeMenu;

    public void Start()
    {
      _graphService = FindObjectOfType<GraphService>();
      edgeMenu.SetActive(false);
    }

    public void OpenContextMenu(UnityEngine.GameObject gameObjectHit)
    {
      var edge = _graphService.FindEdgeByGameObject(gameObjectHit);
      _edge = edge;
      edgeMenu.SetActive(true);
      GameService.Instance.PauseGameWithoutResume();
    }

    public void ChangeParametersButtonOnClick()
    {
      FindObjectOfType<LabelMenuHandler>().OpenLabelMenu(_edge);
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
