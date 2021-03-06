namespace GUIPanels
{
  public static class Rendering
  {
    static IUIRenderer _uiRenderer;
    public static IUIRenderer UIRenderer
    {
      get
      {
        if (_uiRenderer == null)
        {
          _uiRenderer = UnityEngine.GameObject.FindObjectOfType<UIRenderer>();
          if (_uiRenderer == null)
          {
            _uiRenderer = new UnityEngine.GameObject("_uiRenderer")
              .AddComponent<UIRenderer>();
          }
        }
        return _uiRenderer;
      }
    }


    public static void SetImmediateMode(bool value)
    {
      Renderer.Current.ImmediateMode = value;
    }

    public static void PushZ()
    {
      UIRenderer.PushZ();
    }

    public static void PopZ()
    {
      UIRenderer.PopZ();
    }

    public static void BeginTriangleShape(Col color)
    {
      Renderer.Current.BeginTriangleShape(color);

    }
    public static void EndShape()
    {
      Renderer.Current.EndShape();

    }

    public static void AddVertex(float x, float y, float z = -10)
    {
      Renderer.Current.AddVertex(x, y, z);

    }
    public static void DrawTriangle(Rectangle rect, Col color)
    {
      Renderer.Current.DrawTriangle(rect, color);
    }
    public static void DrawTriangle(Vec2 a, Vec2 b, Vec2 c, Col color)
    {
      Renderer.Current.DrawTriangle(a, b, c, color);
    }
    public static void DrawTexture(Rectangle rectangle, Texture texture)
    {
      Renderer.Current.DrawTexture(rectangle, texture);
    }
    public static void DrawRect(Rectangle rectangle, Col color)
    {
      Renderer.Current.DrawRect(rectangle, color);
    }
    public static void DrawText(Rectangle rectangle, string text, ElementStyle style)
    {
      Renderer.Current.DrawText(rectangle, text, style);
    }
    public static void DrawLineBox(Rectangle rect, float width, Col color)
    {
      Renderer.Current.DrawLineBox(rect, width, color);
    }
    public static void DrawLine(Vec2 p0, Vec2 p1, float width, Col color, bool isRounded = false)
    {
      Renderer.Current.DrawLine(p0, p1, width, color, isRounded);
    }
    public static void DrawArc(Vec2 center, float radius, float startAngle, float endAngle, float width, Col color)
    {
      Renderer.Current.DrawArc(center, radius, startAngle, endAngle, color);
    }
    public static void DrawCircle(Vec2 center, float radius, Col color)
    {
      Renderer.Current.DrawCircle(center, radius, color);
    }
    public static void DrawRing(Vec2 center, float radius, float innerRadius, Col color)
    {
      Renderer.Current.DrawRing(center, radius, innerRadius, color);
    }
    public static void DrawArcRing(Vec2 center, float radius, float innerRadius, float startAngle, float endAngle, Col color)
    {
      Renderer.Current.DrawArcRing(center, radius, innerRadius, startAngle, endAngle, color);
    }
  }
  public abstract class Renderer
  {
    public abstract bool ImmediateMode { get; set; }
    public abstract void BeginTriangleShape(Col color);
    public abstract void EndShape();
    public abstract void AddVertex(float x, float y, float z);

    public abstract void DrawTriangle(Rectangle rect, Col color);
    public abstract void DrawTriangle(Vec2 a, Vec2 b, Vec2 c, Col color);

    public int CircleResolution = 20;
    public abstract void DrawLine(Vec2 p0, Vec2 p1, float width, Col color, bool isRounded);
    public abstract void DrawLineBox(Rectangle rect, float width, Col color);
    public abstract void DrawArc(Vec2 center, float radius, float startAngle, float endAngle, Col color);
    public abstract void DrawCircle(Vec2 center, float radius, Col color);
    public abstract void DrawRing(Vec2 center, float radius, float innerRadius, Col color);
    public abstract void DrawArcRing(Vec2 center, float radius, float innerRadius, float startAngle, float endAngle, Col color);

    public abstract void DrawTexture(Rectangle rectangle, Texture texture);
    public abstract void DrawRect(Rectangle rectangle, Col color);
    static Renderer _renderer;

    public abstract void DrawText(Rectangle rectangle, string text, ElementStyle style);

    public static Renderer Current
    {
      get
      {
        if (_renderer == null)
        {
          _renderer = new Unity.Renderer();
        }
        return _renderer;
      }
    }
  }
}
