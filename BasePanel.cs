using System.Collections.Generic;

namespace GUIPanels
{

  public abstract class BasePanel
  {
    public virtual Vec2 Position { get; set; }

    public virtual void Add(IParameter param)
    {
      _parameters.Add(param);
      param.Owner = this;
    }
    public abstract float PaddingLine { get; }
    protected abstract float PaddingX { get; }
    protected abstract float PaddingY { get; }
    protected abstract float PaddingTop { get; }
    public abstract Style Style { get; set; }
    public abstract float TextSize { get; }
    public abstract Col TextColor { get; }
    public abstract float Width { get; }

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
    protected float UpdateParametersCoordinates(float x, float y)
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

    protected virtual void UpdateTotalHeight()
    {
      _totalHeight = WindowBoxStartY;
      _totalHeight += PaddingTop;
      _totalHeight += UpdateParametersCoordinates(Position.x, StartY + PaddingTop);
    }

    public virtual void Draw()
    {
      UpdateEvents();

      UpdateTotalHeight();
      DrawWindowBox();
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
