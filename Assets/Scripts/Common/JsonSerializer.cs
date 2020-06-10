using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common
{

  public static class JsonSerializer
  {
    public static string Serialize(object obj) => JsonUtility.ToJson(obj);

    public static T Deserialize<T>(string json)
    {
      var obj = JsonUtility.FromJson<T>(json);

      CallOnDeserializedMethodRecursively(obj);

      return obj;
    }

    private static void CallOnDeserializedMethodRecursively(object obj)
    {
      if (obj == null)
        return;

      var type = obj.GetType();
      
      var innerFields = type
        .GetFields()
        .Where(p => p.FieldType.IsClass
                    && typeof(IEnumerable).IsAssignableFrom(p.FieldType) == false);


      foreach (var propertyInfo in innerFields)
      {
        CallOnDeserializedMethodRecursively(propertyInfo.GetValue(obj));
      }

      var innerCollections = type
        .GetFields()
        .Where(p => typeof(IEnumerable).IsAssignableFrom(p.FieldType));

      foreach (var fieldInfo in innerCollections)
      {
        if (fieldInfo.GetValue(obj) is IEnumerable<object> collection)
          foreach (var value in collection)
            CallOnDeserializedMethodRecursively(value);
      }

      var methodInfo = type
        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
        .SingleOrDefault(m => m.GetCustomAttributes(typeof(OnDeserializedAttribute), false).Any());

      if (methodInfo != null)
        methodInfo.Invoke(obj, new object[] { });
    }
  }
}
