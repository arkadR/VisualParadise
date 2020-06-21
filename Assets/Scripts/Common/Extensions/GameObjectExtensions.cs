using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Common.Extensions
{
  public static class GameObjectExtensions
  {
    private static readonly int emissionColor = Shader.PropertyToID("_EmissionColor");
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

    public static void ToggleGlow(this UnityEngine.GameObject gameObject)
    {
      var material = LookupMaterial(gameObject);
      if (material.IsKeywordEnabled(emission))
      {
        DisableGlow(gameObject);
      }
      else
      {
        EnableGlow(gameObject);
      }
    }

    public static void SetGlow(this UnityEngine.GameObject gameObject, Color color)
    {
      var physicalNodeMaterial = LookupMaterial(gameObject);
      physicalNodeMaterial.SetColor(emissionColor, color);
    }

    private static Material LookupMaterial(UnityEngine.GameObject gameObject)
    {
      return gameObject.GetComponent<Renderer>().material;
    }

    public static T[] GetAllComponentsOfType<T>(this GameObject gameObject)
    {
      return gameObject
        .GetComponentsInChildren<T>()
        .Union(gameObject.GetComponents<T>())
        .ToArray();
    }
  }
}
