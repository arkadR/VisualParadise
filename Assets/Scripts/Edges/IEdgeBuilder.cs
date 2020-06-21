using System;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Edges
{
  public interface IEdgeBuilder
  {
    IEdgeBuilder BetweenNodes(Node node1, Node node2);
    IEdgeBuilder OnOneNode(Node node);
    IEdgeBuilder Curved(float rotation);
    IEdgeBuilder WithLabel(Func<GameObject> labelFactory, string label, bool visibility);
    IEdgeBuilder WithClass(GraphElementClass @class);
    IEdgeBuilder WithStartLineClass(GraphElementClass @class);
    IEdgeBuilder WithEndLineClass(GraphElementClass @class);
    void BuildOn(Edge edge);
  }
}
