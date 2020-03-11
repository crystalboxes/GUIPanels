using System.Collections.Generic;

namespace GUIPanels
{

  public class BasePanelSettings
  {
    public float PaddingLine = 4,
    PaddingX = 5, PaddingY = 6,
    PaddingTop = 2,
    Width = 135,
    TextSize = 9,
    HeaderHeight = 13;
    public Col TextColor = Col.black;
    public bool ShowTitle = false;
  }

  public abstract class BasePanel
  {
    protected virtual string Name
    {
      get; set;
    }
    BasePanelSettings _basePanelSettings;
    public BasePanel(BasePanelSettings settings, string name = "")
    {
      _basePanelSettings = settings;
      Name = name;

      // memory leak here
      Style = Utils.MakeStyle();
      if (Style != null)
      {
        Style.FontColor = settings.TextColor;
        Style.FontSize = settings.TextSize;
      }
    }
    public virtual Vec2 Position { get; set; }
    public virtual void Add(IParameter param)
    {
      _parameters.Add(param);
      param.Owner = this;
    }
    public virtual float PaddingLine { get { return _basePanelSettings.PaddingLine; } }
    protected virtual float PaddingX { get { return _basePanelSettings.PaddingX; } }
    protected virtual float PaddingY { get { return _basePanelSettings.PaddingY; } }
    protected virtual float PaddingTop { get { return _basePanelSettings.PaddingTop; } }
    public virtual Style Style { get; set; }
    public virtual float TextSize { get { return Style.FontSize; } }
    public virtual Col TextColor { get { return Style.FontColor; } }
    public virtual float Width { get { return _basePanelSettings.Width; } }

    protected virtual float WindowBoxHeight { get { return _totalHeight; } }
    protected virtual float WindowBoxStartY { get { return 0; } }

    protected float ParameterWidth
    {
      get { return Width - PaddingX * 2; }
    }
    protected Rectangle WindowBoxRect
    {
      get { return new Rectangle(Position.x, Position.y + WindowBoxStartY, Width, WindowBoxHeight); }
    }
    protected float _totalHeight = 0;
    protected List<IParameter> _parameters = new List<IParameter>();
    protected virtual float UpdateParametersCoordinates(float x, float y)
    {
      float totalHeight = 0;

      foreach (var parameter in _parameters)
      {
        if (parameter.HideWhen())
        {
          continue;
        }
        parameter.Position = new Vec2(x + PaddingX, y);
        parameter.Width = ParameterWidth;
        y += PaddingY + parameter.Height;
        totalHeight += PaddingY + parameter.Height;
      }

      return totalHeight;
    }
    protected virtual float StartY
    {
      get { return Position.y + WindowBoxStartY; }
    }

    protected virtual float TitleHeight
    {
      get { return _basePanelSettings.ShowTitle ? _basePanelSettings.HeaderHeight + PaddingY : 0; }
    }

    protected virtual void UpdateTotalHeight()
    {
      _totalHeight = WindowBoxStartY;
      _totalHeight += PaddingTop;
      // add header
      _totalHeight += TitleHeight;

      _totalHeight += UpdateParametersCoordinates(Position.x, StartY + PaddingTop + TitleHeight);
    }

    public virtual void Draw()
    {
      UpdateEvents();

      UpdateTotalHeight();
      DrawWindowBox();

      // Draw header

      DrawParameters();
    }

    protected abstract void DrawWindowBox();
    protected virtual void DrawParameters()
    {
      foreach (var parameter in _parameters)
      {
        if (parameter.HideWhen())
        {
          continue;
        }
        parameter.Repaint();
      }
    }

    bool _mouseUp;
    bool _mouseUpPrev;

    public bool MouseUp
    {
      get { return _mouseUp && !_mouseUpPrev; }
    }
    protected void UpdateEvents()
    {
      _mouseUpPrev = _mouseUp;
      _mouseUp = Utils.GetMouseUp();
    }
  }
}
