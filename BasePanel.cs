using System.Collections.Generic;

namespace GUIPanels
{
  public abstract class BasePanel
  {
    protected List<IParameter> _parameters = new List<IParameter>();
    protected Vec2 Position;
    protected abstract float Width { get; }


    public void Add(IParameter param)
    {
      _parameters.Add(param);
      param.Owner = this;
      param.UpdateStyle();
    }


    public virtual void Draw()
    {
      // Need to update position
      UpdateTotalHeight();
      DrawHeader();
      DrawWindowBox();
      DrawParameters();
    }

    protected float _totalHeight = 0;
    public abstract float TextSize { get; }
    public abstract Col TextColor { get; }
    public abstract float PaddingLine { get; }


    protected abstract float HeaderHeight { get; }
    protected abstract float HeaderOffset { get; }
    protected abstract float StartY { get; }
    protected abstract float PaddingX { get; }
    protected abstract float PaddingY { get; }
    protected abstract float PaddingTop { get; }
    protected float WindowBoxHeight { get { return _totalHeight - HeaderHeight - HeaderOffset; } }
    protected float WindowBoxStartY { get { return HeaderHeight + HeaderOffset; } }
    protected float ParameterWidth { get { return Width - PaddingX * 2; } }
    protected Rectangle HeaderRect { get { return new Rectangle(Position.x, Position.y, Width, HeaderHeight); } }
    protected Rectangle WindowBoxRect
    {
      get
      {
        return new Rectangle(Position.x, Position.y + WindowBoxStartY, Width, WindowBoxHeight);
      }
    }

    protected abstract void DrawHeader();

    protected abstract void DrawWindowBox();
    protected void UpdateTotalHeight()
    {
      _totalHeight = 0;
      // header height
      _totalHeight += HeaderHeight;
      // add padding
      _totalHeight += HeaderOffset;
      _totalHeight += PaddingTop;
      // parameters
      float x = Position.x;
      float y = StartY;
      foreach (var parameter in _parameters)
      {
        parameter.Position = new Vec2(x + PaddingX, y);
        parameter.Width = ParameterWidth;
        y += PaddingY + parameter.Height;
        _totalHeight += PaddingY + parameter.Height;
      }
    }

    void DrawParameters()
    {
      foreach (var parameter in _parameters)
      {
        parameter.Repaint();
      }
    }

  }
}