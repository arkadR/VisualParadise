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
    /// Sets correct position of labels. Without that, text is stuck on screen in one place.
    /// </summary>
    void UpdateLabelPositions()
    {
      graphService.Graph?.nodes.ForEach(n => n.UpdateTextPosition());
      graphService.Graph?.edges.ForEach(e => e.UpdateTextPosition());
    }
  }
}
