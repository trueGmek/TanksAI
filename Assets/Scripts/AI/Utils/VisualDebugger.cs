using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace AI.Utils
{
  [RequireComponent(typeof(Renderer))]
  public class VisualDebugger : MonoBehaviour
  {
    private Renderer _renderer;
    private Color _defaultColor;

    private void Awake()
    {
      _renderer = GetComponent<Renderer>();
      _defaultColor = _renderer.material.color;
    }

    public void SetColor(Color color)
    {
      _renderer.material.color = color;
    }

    public void ResetColor()
    {
      SetColor(_defaultColor);
    }
  }
}