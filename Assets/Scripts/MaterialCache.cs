using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts
{
  public class MaterialCache : MonoBehaviour
  {
    const string c_defaultTexturePath = "Textures/node/sand_01_diff_1k.jpg";

    public IDictionary<int, Material> cache = new Dictionary<int, Material>();

    public void Load(IEnumerable<NodeClass> classes)
    {
      foreach (var nodeClass in classes)
      {
        var texture = new Texture2D(2, 2);
        if (nodeClass.texturePath == null)
          texture = LoadDefaultTexture();
        else if (File.Exists(nodeClass.texturePath))
        {
          var fileData = File.ReadAllBytes(nodeClass.texturePath);
          texture.LoadImage(fileData);
        }
        else
        {
          Log($"Texture {nodeClass.texturePath} was not found!");
          texture = LoadDefaultTexture();
        }

        var material = new Material(Shader.Find("Standard"))
        {
          mainTexture = texture
        };
        cache.Add(nodeClass.id, material);
      }
    }

    public Material GetByClassId(int id) => cache[id];

    Texture2D LoadDefaultTexture() => Resources.Load<Texture2D>(c_defaultTexturePath);

    void Log(string message)
    {
      Debug.Log(message);
    }
  }
}
