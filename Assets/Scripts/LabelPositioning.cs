using UnityEngine;

namespace Assets.Scripts
{
  class LabelPositioning : MonoBehaviour
  {
    GraphService graphService;
    public void Start() => graphService = FindObjectOfType<GraphService>();

    public void FixedUpdate()
    {
      if (GameService.Instance.IsPaused)
        return;

      if (graphService.LabelVisibility)
        UpdateLabelPositions();
    }

    /// <summary>
    /// Sets correct Position of labels. Without that, text is stuck on screen in one place.
    /// </summary>
    void UpdateLabelPositions()
    {
      var camera = Camera.main;
      graphService.Graph?.Nodes.ForEach(n => n.UpdateTextPosition(camera));
      graphService.Graph?.Edges.ForEach(e => e.UpdateTextPosition(camera));
    }
  }
}
