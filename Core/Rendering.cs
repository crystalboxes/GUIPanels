namespace GUIPanels
{
  public static class Rendering
  {
    public static void DrawTexture(Rectangle rectangle, Texture texture)
    {
      Renderer.Current.DrawTexture(rectangle, texture);
    }
    public static void DrawRect(Rectangle rectangle, Col color)
    {
      Renderer.Current.DrawRect(rectangle, color);
    }
    public static void DrawText(Rectangle rectangle, string text, Style style)
    {
      Renderer.Current.DrawText(rectangle, text, style);
    }
    public static void DrawLine(Vec2 p0, Vec2 p1, float width, Col color)
    {
      Renderer.Current.DrawLine(p0, p1, width, color);
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
    public int CircleResolution = 20;
    public abstract void DrawLine(Vec2 p0, Vec2 p1, float width, Col color);
    public abstract void DrawArc(Vec2 center, float radius, float startAngle, float endAngle, Col color);
    public abstract void DrawCircle(Vec2 center, float radius, Col color);
    public abstract void DrawRing(Vec2 center, float radius, float innerRadius, Col color);
    public abstract void DrawArcRing(Vec2 center, float radius, float innerRadius, float startAngle, float endAngle, Col color);

    public abstract void DrawTexture(Rectangle rectangle, Texture texture);
    public abstract void DrawRect(Rectangle rectangle, Col color);
    static Renderer _renderer;

    public abstract void DrawText(Rectangle rectangle, string text, Style style);

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