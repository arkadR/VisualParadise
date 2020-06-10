﻿using UnityEngine;

namespace Assets.Scripts
{
  public class NodeGameObjectFactory : MonoBehaviour
  {
    public GameObject nodePrefab;

    public GameObject CreateNodeGameObject(Vector3 position, Quaternion rotation) =>
      Instantiate(nodePrefab, position, rotation);
  }
}
