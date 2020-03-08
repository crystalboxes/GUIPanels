using UnityEngine;

namespace GUIPanels
{
  public static partial class Utils
  {
    public static Col HSVToRGB(float h, float s, float v)
    {
      return Color.HSVToRGB(h, s, v);
    }
    public static void RGBToHSV(Col color, out float h, out float s, out float v)
    {
      Color.RGBToHSV(color, out h, out s, out v);
    }

    public static Texture MakeTexture(int width, int height)
    {
      return new Unity.UnityTexture(width, height);
    }
    public static Style MakeStyle()
    {
      return new Unity.UnityStyle();
    }
    public static Vec2 MousePosition()
    {
      var mp = Input.mousePosition;
      return new Vec2(
        Aspect.UnAdjust(mp.x),
        Aspect.UnAdjust(Screen.height - mp.y)
      );
    }
    public static bool GetMouseButton()
    {
      return Input.GetMouseButton(0);
    }
    public static bool GetMouseDown()
    {
      return Input.GetMouseButtonDown(0);
    }
    public static bool GetMouseUp()
    {
      return Input.GetMouseButtonUp(0);
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
      return new Rect(
        Aspect.Adjust(rect.x),
        Aspect.Adjust(rect.y),
        Aspect.Adjust(rect.width),
        Aspect.Adjust(rect.height)
      );
    }
  }
  namespace Unity
  {
    public class UnityTexture : GUIPanels.Texture
    {
      public override void SetPixel(int x, int y, Col color)
      {
        Tex.SetPixel(x, y, color);
      }

      public override void Apply()
      {
        Tex.Apply();
      }

      public Texture2D Tex { get { return _texture; } }
      Texture2D _texture;
      public UnityTexture(int width, int height)
      {
        _texture = new Texture2D(width, height, TextureFormat.ARGB32, false, false);
      }
    }
    public class UnityStyle : Style
    {
      GUIStyle _style;
      public GUIStyle GUIStyle { get { return _style; } }
      public UnityStyle()
      {
        _style = new GUIStyle(GUIStyle.none);
        Aspect.OnScaleChange += () => FontSize = _fontSize;
      }

      float _fontSize = 14f;

      public override float FontSize
      {
        get
        {
          return _fontSize;
        }

        set
        {
          _fontSize = value;
          _style.fontSize = (int)(Aspect.Adjust(_fontSize));
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
    public partial class Renderer : GUIPanels.Renderer
    {
      public override void DrawTexture(Rectangle rectangle, Texture texture)
      {
        GUI.DrawTexture(Utils.ToRect(rectangle), (texture as UnityTexture).Tex);
      }

      public override void DrawText(Rectangle rectangle, string text, Style style)
      {
        var st = style as UnityStyle;
        GUI.Label(Utils.ToRect(rectangle), text, st.GUIStyle);
      }
    }
  }
}