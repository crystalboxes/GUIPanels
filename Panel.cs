using System.Collections.Generic;
namespace GUIPanels
{
  public struct PanelSettings
  {
    public float Width;
    public float PaddingLine, PaddingY, PaddingX, PaddingTop, HeaderOffset;
    public Col HeaderColor;
    public Col BackgroundColor;
    public Col TextColor;
    public float TextSize;
    public float HeaderHeight;
    public bool ShowTitle;

    public static PanelSettings Default
    {
      get
      {
        return new PanelSettings()
        {
          Width = 200,
          PaddingLine = 4,
          PaddingTop = 2,
          HeaderOffset = 2,
          PaddingY = 6,
          PaddingX = 5,
          HeaderColor = new Col(0.5f, 0.5f, 0.5f, 0.8f),
          BackgroundColor = new Col(1, 1, 1, 0.3f),
          TextColor = Col.white,
          TextSize = 14,
          HeaderHeight = 13,
          ShowTitle = true,
        };
      }
    }
  }

  public class Panel : BasePanel
  {
    protected override float Width { get { return _settings.Width; } }

    public Panel(PanelSettings settings)
    {
      _settings = settings;
    }
    public Panel()
    {
      _settings = PanelSettings.Default;
    }

    PanelSettings _settings;

    public void SetPosition(float x, float y)
    {
      Position.x = x;
      Position.y = y;
    }


    public override void Draw()
    {
      // Need to update position
      HandleDragging();
      base.Draw();
    }


    public override float TextSize { get { return _settings.TextSize; } }
    public override Col TextColor { get { return _settings.TextColor; } }
    public override float PaddingLine { get { return _settings.PaddingLine; } }

    protected override float HeaderHeight { get { return _settings.HeaderHeight; } }
    protected override float StartY { get { return Position.y + WindowBoxStartY + _settings.PaddingTop; } }
    protected override float PaddingTop { get { return _settings.PaddingTop; } }

    protected override float PaddingY { get { return _settings.PaddingY; } }
    protected override float PaddingX { get { return _settings.PaddingX; } }


    protected override float HeaderOffset { get { return _settings.HeaderOffset; } }


    protected override void DrawHeader()
    {
      Renderer.Current.DrawRect(HeaderRect, _settings.HeaderColor);
    }

    protected override void DrawWindowBox()
    {
      Renderer.Current.DrawRect(WindowBoxRect, _settings.BackgroundColor);
    }


    bool _mouseDown;
    Vec2 _offset;
    void HandleDragging()
    {
      var hr = HeaderRect;
      var mp = Utils.MousePosition();
      if (hr.Contains(mp) && Utils.GetMouseButton() && !_mouseDown)
      {
        _offset = Position - mp;
        _mouseDown = true;
      }

      if (_mouseDown)
      {
        Position = mp + _offset;
        _mouseDown = Utils.GetMouseButton();
      }

    }
  }
}
