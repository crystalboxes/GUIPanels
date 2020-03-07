using System.Collections.Generic;

namespace GUIPanels
{
  public abstract class BasePanel
  {
    public virtual Vec2 Position { get; set; }
    protected List<IParameter> _parameters = new List<IParameter>();
    protected virtual string Name
    {
      get { return ""; }
    }
    public abstract float Width { get; }

    public virtual void Add(IParameter param)
    {
      _parameters.Add(param);
      param.Owner = this;
    }

    bool _mouseUp;
    bool _mouseUpPrev;

    public bool MouseUp
    {
      get { return _mouseUp && !_mouseUpPrev; }
    }

    bool _collapsed = false;

    void UpdateEvents()
    {
      _mouseUpPrev = _mouseUp;
      _mouseUp = Utils.GetMouseUp();
    }

    float HeaderTextSize
    {
      get { return HeaderHeight * 0.65f; }
    }

    protected virtual Col HeaderColor
    {
      get { return Col.black; }
    }

    protected Rectangle CollapsedButtonRect
    {
      get
      {
        float size = HeaderTextSize;
        float padding = (HeaderHeight - size) / 2f;
        float xOffset = TextSize * 1.11f;
        var collapseButtonPosition = HeaderRect;
        collapseButtonPosition.width = xOffset;
        collapseButtonPosition.y += padding;
        collapseButtonPosition.height = size;
        collapseButtonPosition.x = collapseButtonPosition.x + HeaderRect.width - xOffset - padding;
        return collapseButtonPosition;
      }
    }

    Style collapsedButtonStyle = null;

    protected Style CollapsedButtonStyle
    {
      get
      {
        if (collapsedButtonStyle == null)
        {
          collapsedButtonStyle = Utils.MakeStyle();
          collapsedButtonStyle.FontSize = HeaderTextSize;
          collapsedButtonStyle.FontColor = HeaderTextColor;
        }

        return collapsedButtonStyle;
      }
    }

    public virtual void Draw()
    {
      UpdateEvents();
      // Need to update position
      if (CollapsedButtonRect.Contains(Utils.MousePosition()) && MouseUp)
      {
        _collapsed = !_collapsed;
      }

      UpdateTotalHeight();
      DrawHeader();

      if (_collapsed) return;
      DrawWindowBox();
      DrawParameters();
    }

    protected virtual Col HeaderTextColor
    {
      get { return Col.black; }
    }

    protected void DrawCollapsedButton()
    {
      var collapsedButtonRect = CollapsedButtonRect;
      Renderer.Current.DrawText(collapsedButtonRect, _collapsed ? "[+]" : "[ - ]", CollapsedButtonStyle);
    }

    protected virtual void DrawHeader()
    {
      Renderer.Current.DrawRect(HeaderRect, HeaderColor);

      var collapsedButtonRect = CollapsedButtonRect;
      DrawCollapsedButton();
      if (Name.Length <= 0) return;
      collapsedButtonRect.x = Position.x + PaddingX;
      Renderer.Current.DrawText(collapsedButtonRect, Name, CollapsedButtonStyle);
    }

    protected float _totalHeight = 0;
    public abstract float TextSize { get; }
    public abstract Col TextColor { get; }
    public abstract float PaddingLine { get; }

    public abstract Style Style { get; set; }

    protected abstract float HeaderHeight { get; }
    protected abstract float HeaderOffset { get; }

    protected virtual float StartY
    {
      get { return Position.y + WindowBoxStartY; }
    }

    protected abstract float PaddingX { get; }
    protected abstract float PaddingY { get; }
    protected abstract float PaddingTop { get; }

    protected float WindowBoxHeight
    {
      get { return _totalHeight - HeaderHeight - HeaderOffset; }
    }

    protected float WindowBoxStartY
    {
      get { return HeaderHeight + HeaderOffset; }
    }

    protected float ParameterWidth
    {
      get { return Width - PaddingX * 2; }
    }

    protected Rectangle HeaderRect
    {
      get { return new Rectangle(Position.x, Position.y, Width, HeaderHeight); }
    }

    protected Rectangle WindowBoxRect
    {
      get { return new Rectangle(Position.x, Position.y + WindowBoxStartY, Width, WindowBoxHeight); }
    }


    protected abstract void DrawWindowBox();

    protected void UpdateTotalHeight()
    {
      _totalHeight = 0;
      // header height
      _totalHeight += HeaderHeight;
      // add padding
      if (_collapsed) return;
      _totalHeight += HeaderOffset;
      _totalHeight += PaddingTop;
      // parameters
      float x = Position.x;
      float y = StartY + PaddingTop;
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