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
          PaddingTop = 0,
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
    public override float Width
    {
      get { return _settings.Width; }
    }

    string _name;

    protected override string Name
    {
      get { return _name; }
    }

    public Panel(PanelSettings settings, string name = "")
    {
      _name = name;
      _settings = settings;
      SetStyle();
    }

    public Panel(string name = "")
    {
      _settings = PanelSettings.Default;
      SetStyle();
    }

    void SetStyle()
    {
      Style = Utils.MakeStyle();
      Style.FontColor = _settings.TextColor;
      Style.FontSize = _settings.TextSize;
    }

    PanelSettings _settings;

    public void SetPosition(float x, float y)
    {
      Position = new Vec2(x, y);
    }


    public override void Draw()
    {
      // Need to update position
      HandleDragging();
      base.Draw();
    }

    public override Style Style { get; set; }

    public override float TextSize
    {
      get { return _settings.TextSize; }
    }

    public override Col TextColor
    {
      get { return _settings.TextColor; }
    }

    public override float PaddingLine
    {
      get { return _settings.PaddingLine; }
    }

    protected override float HeaderHeight
    {
      get { return _settings.HeaderHeight; }
    }

    protected override float StartY
    {
      get { return base.StartY + _settings.PaddingTop; }
    }

    protected override float PaddingTop
    {
      get { return _settings.PaddingTop; }
    }

    protected override float PaddingY
    {
      get { return _settings.PaddingY; }
    }

    protected override float PaddingX
    {
      get { return _settings.PaddingX; }
    }

    protected override Col HeaderColor
    {
      get { return _settings.HeaderColor; }
    }


    protected override float HeaderOffset
    {
      get { return _settings.HeaderOffset; }
    }

    public override void Add(IParameter param)
    {
      base.Add(param);
      param.UpdateStyle();
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
      if (hr.Contains(mp) && Utils.GetMouseDown() && !_mouseDown)
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