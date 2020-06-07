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

    public static Node ZeroNode(int id) => new Node { id = id, point = new Point(), apoint = new APoint(), vpoint = new VPoint() };
  }

  [Serializable]
  public class Point
  {
    public float x, y, z, theta, phi, psi;

    public Vector3 Position() => new Vector3(x, y, z);
    public Quaternion Rotation() => Quaternion.Euler(theta, phi, psi);
    public void SetPosition(Vector3 position)
    {
      x = position.x;
      y = position.y;
      z = position.z;
    }
  }

  [Serializable]
  public class VPoint
  {
    public float vx, vy, vz, om_theta, om_phi, om_psi;

    public Vector3 Velocity() => new Vector3(vx, vy, vz);
    public Vector3 AngVelocity() => new Vector3(om_theta, om_phi, om_psi);

    public void SetVelocity(Vector3 velocity)
    {
      vx = velocity.x;
      vy = velocity.y;
      vz = velocity.z;
    }

    public void SetAngVelocity(Vector3 angVelocity)
    {
      om_theta = angVelocity.x;
      om_phi = angVelocity.y;
      om_psi = angVelocity.z;
    }
  }

  [Serializable]
  public class APoint
  {
    public float ax, ay, az, alpha_theta, alpha_phi, alpha_psi;

    public Vector3 Acceleration() => new Vector3(ax, ay, az);
    public Vector3 AngAcceleration() => new Vector3(alpha_theta, alpha_phi, alpha_psi);
  }

  [Serializable]
  public class Edge
  {
    public int from;
    public int to;
    public int weight;
  }
}
