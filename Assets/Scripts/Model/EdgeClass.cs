using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Assets.Scripts.Model
{
  [Serializable]
  public class EdgeClass
  {
    [JsonProperty] public int Id { get; private set; }
    [JsonProperty] public string Name { get; private set; }
    [JsonProperty] public string TexturePath { get; private set; }
    [JsonProperty] public LineEnding LineStart { get; private set; } = LineEnding.None;
    [JsonProperty] public LineEnding LineEnd { get; private set; } = LineEnding.None;
    [JsonProperty] public float? Thickness { get; private set; }

    [OnDeserialized]
    internal void OnDeserialized(StreamingContext context)
    {
      if (string.IsNullOrEmpty(Name))
        Name = Id.ToString();
    } 
  }
}
