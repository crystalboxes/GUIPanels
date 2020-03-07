using UnityEngine;

namespace GUIPanels
{
  public static class Utils
  {
    public static Style MakeStyle()
    {
      return new Unity.UnityStyle();
    }
    public static Vec2 MousePosition()
    {
      var mp = Input.mousePosition;
      return new Vec2(mp.x, Screen.height - mp.y);
    }
    public static bool GetMouseButton()
    {
      return Input.GetMouseButton(0);
    }
    public static Col ToCol(Color col)
    {
      return new Col(col.r, col.g, col.b, col.a);
    }
    public static Color FromCol(Col col)
    {
      return new Color(col.r, col.g, col.b, col.a);
    }
    public static Rect ToRect(Rectangle rect)
    {
      return new Rect(rect.x, rect.y, rect.width, rect.height);
    }
  }
  namespace Unity
  {
    public class UnityStyle : Style
    {
      GUIStyle _style;
      public GUIStyle GUIStyle { get { return _style; } }
      public UnityStyle()
      {
        _style = GUIStyle.none;
      }

      public override float FontSize
      {
        get
        {
          return _style.fontSize;
        }

        set
        {
          _style.fontSize = (int)value;
        }
      }

      public override Col FontColor
      {
        get
        {
          return Utils.ToCol(_style.normal.textColor);
        }
        set
        {
          _style.normal.textColor = Utils.FromCol(value);
        }
      }
    }
    public class Renderer : GUIPanels.Renderer
    {
      class RectTexture
      {
        public Texture2D GetTexture(Color col)
        {
          if (_currentColor != col)
          {
            _currentColor = col;
            _texture.SetPixel(0, 0, _currentColor);
            _texture.Apply();
          }
          return _texture;
        }
        Texture2D _texture;
        Color _currentColor;
        public RectTexture()
        {
          _currentColor = Color.white;
          _texture = new Texture2D(1, 1);
          _texture.SetPixel(0, 0, _currentColor);
          _texture.Apply();
        }
      }
      static RectTexture _rectTexture = null;
      static RectTexture RectTex
      {
        get
        {
          if (_rectTexture == null)
          {
            _rectTexture = new RectTexture();
          }
          return _rectTexture;
        }
      }

      public override void DrawRect(Rectangle rectangle, Col color)
      {
        GUI.DrawTexture(Utils.ToRect(rectangle), RectTex.GetTexture(Utils.FromCol(color)));
      }

      public override void DrawText(Rectangle rectangle, string text, Style style)
      {
        var st = style as UnityStyle;
        GUI.Label(Utils.ToRect(rectangle), text, st.GUIStyle);
      }
    }
  }
}