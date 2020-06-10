using System;
using System.Reflection.Emit;
using Assets.Scripts.Common.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class Node
  {
    public int id;
    public string label;
    public Point point;
    public VPoint vpoint;
    public APoint apoint;

    [NonSerialized] public GameObject gameObject;

    public static Node EmptyNode(int id, GameObject gameObject)
    {
      var node = new Node
      {
        id = id,
        label = id.ToString(),
        point = new Point(),
        apoint = new APoint(),
        vpoint = new VPoint(),
        gameObject = gameObject
      };
      node.gameObject.GetComponent<Text>().text = node.label;
      return node;
    }

    public Vector3 Position
    {
      get => point.position;
      set
      {
        point.position = value;
        gameObject.transform.position = value;
        gameObject.GetComponentInChildren<Text>().transform.position = Camera.main.WorldToScreenPoint(value);
      }
    }

    public Vector3 Rotation
    {
      get => point.rotation;
      set
      {
        point.rotation = value;
        gameObject.transform.rotation = Quaternion.Euler(value);
      }
    }

    public Vector3 Velocity
    {
      get => vpoint.velocity;
      set => vpoint.velocity = value;
    }

    public Vector3 AngularVelocity
    {
      get => vpoint.angularVelocity;
      set => vpoint.angularVelocity = value;
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
}
