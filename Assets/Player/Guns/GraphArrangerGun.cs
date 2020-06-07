using System.Collections.Generic;
using UnityEngine;
using static GraphLoader;

namespace Player.Guns
{
    public class GraphArrangerGun : IGun
    {
        private readonly GraphArranger graphArranger;

        public GraphArrangerGun(GraphArranger graphArranger)
        {
            this.graphArranger = graphArranger;
        }

        public string GetGunName() => "Arrange";

        public void OnMoveDown(Transform playerTransform, Camera camera)
        {
            graphArranger.HandleArrangeButtonPress();
        }

        public void OnRightClick(Camera camera)
        {
            
        }

        public void OnSwitchedAway()
        {

        }
    }
}