using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Utils
{
  [AttributeUsage(AttributeTargets.Method)]
  [MeansImplicitUse(ImplicitUseKindFlags.Access | ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
  public class ButtonAttribute : PropertyAttribute
  {
    public readonly string ButtonName;

    public ButtonAttribute(string buttonName)
    {
      ButtonName = buttonName;
    }

    public ButtonAttribute()
    {
    }
  }
}