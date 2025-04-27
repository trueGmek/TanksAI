using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
  [CustomEditor(typeof(MonoBehaviour), true)]
  public class ButtonEditor : UnityEditor.Editor
  {
    public override void OnInspectorGUI()
    {
      DrawDefaultInspector();

      MethodInfo[] methods = target.GetType()
        .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

      foreach (MethodInfo method in methods)
      {
        ButtonAttribute buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
        if (buttonAttribute == null)
          continue;
        string buttonName = string.IsNullOrEmpty(buttonAttribute.ButtonName) ? method.Name : buttonAttribute.ButtonName;

        if (GUILayout.Button(buttonName))
        {
          method.Invoke(target, null);
        }
      }
    }
  }
}