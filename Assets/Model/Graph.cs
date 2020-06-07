using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Model
{
  [Serializable]
  public class Graph
  {
    public List<Node> nodes;
    public List<Edge> edges;
  }

  [Serializable]
  public class Node
  {
    public int id;
    public Point point;
    public VPoint vpoint;
    public APoint apoint;

    [NonSerialized] 
    public UnityEngine.GameObject gameObject;

    public static Node EmptyNode(int id, UnityEngine.GameObject gameObject) => new Node
    {
      id = id, 
      point = new Point(), 
      apoint = new APoint(), 
      vpoint = new VPoint(),
      gameObject = gameObject
    };

    public Vector3 Position
    {
      get => point.position;
      set
      {
        point.position = value;
        // gameObject.GetComponent<Rigidbody>().position = value;
        gameObject.transform.position = value;
      }
    }

    public Vector3 Rotation
    {
      get => point.rotation;
      set
      {
        point.rotation = value;
        // gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(value);
        gameObject.transform.rotation = Quaternion.Euler(value);
      }
    }

    public Vector3 Velocity
    {
      get => vpoint.velocity;
      set
      {
        vpoint.velocity = value;
        // gameObject.GetComponent<Rigidbody>().velocity = value;
      }
    }

    public Vector3 AngularVelocity
    {
      get => vpoint.angularVelocity;
      set { 
        vpoint.angularVelocity = value;
        // gameObject.GetComponent<Rigidbody>().angularVelocity = value;
      }
    }

    public Vector3 Acceleration
    {
      get => apoint.acceleration;
      set => apoint.acceleration = value;
    }

    public Vector3 AngularAcceleration
    {
      get => apoint.angularAcceleration;
      set => apoint.angularAcceleration = value;
    }

  }

  [Serializable]
  public class Point
  {
    public Vector3 position;
    public Vector3 rotation;
  }

  [Serializable]
  public class VPoint
  {
    public Vector3 velocity;
    public Vector3 angularVelocity;
  }

  [Serializable]
  public class APoint
  {
    public Vector3 acceleration;
    public Vector3 angularAcceleration;
  }

  [Serializable]
  public class Edge
  {
    public int from;
    public int to;
    public int weight;

    [NonSerialized]
    public UnityEngine.GameObject gameObject;
  }
}
