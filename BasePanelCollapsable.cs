using System.Collections.Generic;

namespace GUIPanels
{
  public abstract class BasePanelCollapsable : BasePanel
  {
    protected virtual string Name
    {
      get { return ""; }
    }



    bool _collapsed = false;

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



    protected abstract float HeaderHeight { get; }
    protected abstract float HeaderOffset { get; }
    float HeaderOffsetChecked { get { return _collapsed ? 0 : HeaderOffset; } }


    protected override float WindowBoxStartY { get { return HeaderHeight + HeaderOffsetChecked; } }

    protected override float WindowBoxHeight
    {
      get { return base.WindowBoxHeight - HeaderHeight - HeaderOffsetChecked; }
    }

    protected Rectangle HeaderRect
    {
      get { return new Rectangle(Position.x, Position.y, Width, HeaderHeight); }
    }



    protected override void UpdateTotalHeight()
    {
      // header height
      _totalHeight = WindowBoxStartY;
      // add padding
      if (_collapsed) return;
      _totalHeight += PaddingTop;
      // parameters
      _totalHeight += UpdateParametersCoordinates(Position.x, StartY + PaddingTop);
    }

  }
}
