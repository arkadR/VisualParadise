﻿using Assets.Scripts.Common.Extensions;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Guns
{
  enum EdgeMode { Create, Delete}
  public class EdgeGun : IGun
  {
    [SerializeField] private float hitDistance = 40f;
    private static readonly Color _createGlowColor = Color.green;
    private static readonly Color _deleteGlowColor = Color.red;
    private readonly GraphService graphService;
    private readonly Material edgeMaterial;
    private EdgeMode _mode = EdgeMode.Create;
    private Node previouslyHitNode;
    private float nodeHitGlowStrength = 0.5f;

    public EdgeGun(GraphService graphService, Material edgeMaterial)
    {
      this.graphService = graphService;
      this.edgeMaterial = edgeMaterial;
    }

    public string GunName => "Edge";

    private EdgeMode Mode
    {
      get => _mode;
      set
      {
        if(_mode != value)
        {
          ClearPreviouslyHitNode();
          _mode = value;
        }
      }
    }

    private void ClearPreviouslyHitNode()
    {
      if (previouslyHitNode != null)
      {
        previouslyHitNode.gameObject.DisableGlow();
        previouslyHitNode = null;
      }
    }

    public void OnMoveDown(Transform playerTransform, Camera camera)
    {
      Mode = EdgeMode.Create;
      OnActonPerformed(camera);
    }

    private void OnActonPerformed(Camera camera)
    {
      var ray = camera.ScreenPointToRay(Input.mousePosition);

      if (!Physics.Raycast(ray, out var hit))
        return;

      var gameObjectHit = hit.collider.gameObject;
      var currentlyHitNode = graphService.FindNodeByGameObject(gameObjectHit);

      // If not a node, don't do anything
      if (currentlyHitNode == null)
        return;

      // If player hit the same node twice, don't do anything
      if (currentlyHitNode == previouslyHitNode)
      {
        previouslyHitNode.gameObject.DisableGlow();
        previouslyHitNode = null;
        return;
      }

      if (previouslyHitNode == null)
      {
        previouslyHitNode = currentlyHitNode;
        previouslyHitNode.gameObject.EnableGlow();
        var glowColor = Mode == EdgeMode.Create ? _createGlowColor : _deleteGlowColor;
        previouslyHitNode.gameObject.SetGlow(glowColor * nodeHitGlowStrength);
        return;
      }

      var existingEdge = graphService.FindEdgeByNodes(previouslyHitNode, currentlyHitNode);

      switch (Mode)
      {
        case EdgeMode.Create:
          {
            //Don't create duplicate edge
            if (existingEdge != null)
              return;

            graphService.AddEdge(currentlyHitNode, previouslyHitNode, edgeMaterial);
            break;
          }
        case EdgeMode.Delete:
          {
            if (existingEdge == null)
              return;
            graphService.RemoveEdge(existingEdge);
            break;
          }
      }

      previouslyHitNode.gameObject.DisableGlow();
      previouslyHitNode = null;
    }

    public void OnSwitchedAway()
    {
      ClearPreviouslyHitNode();
    }

    public void OnRightClick(Camera camera)
    {
      Mode = EdgeMode.Delete;
      OnActonPerformed(camera);
    }
    }
}
