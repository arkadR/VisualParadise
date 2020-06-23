using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts
{
  public class MaterialCache : MonoBehaviour
  {
    const string c_defaultTexturePath = "Textures/node/sand_01_diff_1k.jpg";

    public IDictionary<string, Material> cache = new Dictionary<string, Material>();

    public Lazy<Material> DefaultMaterial = new Lazy<Material>(
      () => new Material(Shader.Find("Standard")) { mainTexture = LoadDefaultTexture() });
    
    public void Load(IEnumerable<GraphElementClass> classes)
    {
      var texturePaths = classes
        .Select(c => c.TexturePath)
        .Where(tp => string.IsNullOrEmpty(tp) == false)
        .Distinct()
        .Where(tp => cache.ContainsKey(tp) == false);

      foreach (var texturePath in texturePaths)
      {
        var texture = new Texture2D(2, 2);
        if (File.Exists(texturePath))
        {
          var fileData = File.ReadAllBytes(texturePath);
          texture.LoadImage(fileData);
        }
        else
        {
          Log($"Texture {texturePath} was not found!");
          texture = LoadDefaultTexture();
        }
        var material = new Material(Shader.Find("Standard")) { mainTexture = texture };
        cache.Add(texturePath, material);
      }
    }

    public Material GetByTexturePath(string texturePath) =>
      string.IsNullOrEmpty(texturePath) ? DefaultMaterial.Value : cache[texturePath];

    static Texture2D LoadDefaultTexture() => Resources.Load<Texture2D>(c_defaultTexturePath);

    void Log(string message)
    {
      Debug.Log(message);
    }
  }
}
