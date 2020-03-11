using System.Collections.Generic;

namespace GUIPanels
{
  public class BasePanelCollapsibleSettings : BasePanelSettings
  {
    public float HeaderOffset = 2;
    public Col HeaderColor = new Col(0.5f, 0.5f, 0.5f, 0.8f);
    public Col HeaderTextColor = Col.black;
    public new bool ShowTitle = true;

  }
  public abstract class BasePanelCollapsible : BasePanel
  {
    BasePanelCollapsibleSettings _basePanelCollapsibleSettings;
    public BasePanelCollapsible(BasePanelCollapsibleSettings settings) : base(settings)
    {
      _basePanelCollapsibleSettings = settings;
    }


    bool _collapsed = false;

    protected virtual float HeaderHeight { get { return _basePanelCollapsibleSettings.HeaderHeight; } }
    protected virtual float HeaderOffset { get { return _basePanelCollapsibleSettings.HeaderOffset; } }

    protected virtual float HeaderTextSize
    {
      get { return HeaderHeight * 0.65f; }
    }

    protected virtual Col HeaderColor
    {
      get { return _basePanelCollapsibleSettings.HeaderColor; }
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

    public override void Draw()
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
      get { return _basePanelCollapsibleSettings.HeaderTextColor; }
    }

    protected void DrawCollapsedButton()
    {
      var collapsedButtonRect = CollapsedButtonRect;
      Rendering.DrawText(collapsedButtonRect, _collapsed ? "[+]" : "[ - ]", CollapsedButtonStyle);
    }

    protected virtual void DrawHeader()
    {
      Rendering.DrawRect(HeaderRect, HeaderColor);

      var collapsedButtonRect = CollapsedButtonRect;
      DrawCollapsedButton();
      if (Name.Length <= 0 || !_basePanelCollapsibleSettings.ShowTitle) return;
      collapsedButtonRect.x = Position.x + PaddingX;
      Rendering.DrawText(collapsedButtonRect, Name, CollapsedButtonStyle);
    }



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
