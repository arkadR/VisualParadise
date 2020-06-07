using UnityEngine;

namespace Player.Guns
{
  public class GraphArrangerGun : IGun
  {
    private readonly GraphArranger graphArranger;

    public GraphArrangerGun(GraphArranger graphArranger)
    {
      this.graphArranger = graphArranger;
    }

    public string GunName => "Arrange";

    public void OnMoveDown(Transform playerTransform, Camera camera)
    {
      graphArranger.ToggleArrangement();
    }

    public void OnRightClick(Camera camera)
    {
        
    }

    public void OnSwitchedAway()
    {

    }
  }
}