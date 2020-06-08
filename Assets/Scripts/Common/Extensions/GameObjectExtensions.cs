using UnityEngine;

namespace Assets.Scripts.Common.Extensions
{
  public static class GameObjectExtensions
  {
    private static readonly int emissionColor = Shader.PropertyToID("_EmissionColor");
    private static readonly Color glowColor = Color.yellow;
    private const string emission = "_EMISSION";

    public static void EnableGlow(this UnityEngine.GameObject gameObject)
    {
      var material = LookupMaterial(gameObject);
      material.EnableKeyword(emission);
    }

    public static void DisableGlow(this UnityEngine.GameObject gameObject)
    {
      var material = LookupMaterial(gameObject);
      material.DisableKeyword(emission);
    }

    public static void SetGlow(this UnityEngine.GameObject gameObject, float glowStrength)
    {
      var physicalNodeMaterial = LookupMaterial(gameObject);
      physicalNodeMaterial.SetColor(emissionColor, glowColor * glowStrength);
    }

    private static Material LookupMaterial(UnityEngine.GameObject gameObject)
    {
      return gameObject.GetComponent<Renderer>().material;
    }
  }
}
