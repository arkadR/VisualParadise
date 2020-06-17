using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class NodeClass
  {
    public int id;
    public string name;
    [JsonConverter(typeof(StringEnumConverter))]
    public PrimitiveType? shape;
    public string texturePath;
    public float? scale;


    [OnDeserialized]
    public void OnDeserialized(StreamingContext context)
    {
      if (string.IsNullOrEmpty(name))
        name = id.ToString();
    }
  }
}
