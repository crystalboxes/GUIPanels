namespace GUIPanels
{
  public abstract class Renderer
  {
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