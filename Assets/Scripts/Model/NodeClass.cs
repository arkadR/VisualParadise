using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class NodeClass
  {
    public int id;
    public string shape;
    [CanBeNull]
    public string texturePath;

    public float? scale;

    [NonSerialized]
    public PrimitiveType? objectType;
    

    [OnDeserialized]
    public void OnDeserialized(StreamingContext context) => objectType = Enum.Parse(typeof(PrimitiveType), shape) as PrimitiveType?;
  }
}
