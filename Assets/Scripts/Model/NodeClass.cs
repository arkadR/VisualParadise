using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class NodeClass
  {
    public int id;
    [JsonConverter(typeof(StringEnumConverter))]
    public PrimitiveType? shape;
    [CanBeNull]
    public string texturePath;

    public float? scale;

  }
}
