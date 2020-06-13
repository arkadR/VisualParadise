using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Common.Utils
{
  public class DebugSphereFactory : MonoBehaviour
  {
    readonly List<GameObject> _debugSpheres = new List<GameObject>();
    public GameObject debugSpherePrefab;

    public void ClearDebugSpheres()
    {
      foreach (var sphere in _debugSpheres)
      {
        Destroy(sphere);
      }

      _debugSpheres.Clear();
    }

    public void AddDebugSphere(Vector3 position) =>
      _debugSpheres.Add(Instantiate(debugSpherePrefab, position, Quaternion.identity));
  }
}
