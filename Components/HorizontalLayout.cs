using System;

namespace GUIPanels
{
  public class HorizontalLayout2 : BasePanel, IParameter
  {
    public HorizontalLayout2(BasePanelSettings settings, string name = "") : base(settings, name)
    {
    }
    public System.Func<bool> HideWhen = () => false;

    Func<bool> IParameter.HideWhen
    {
      get
      {
        return this.HideWhen;
      }

      set
      {
        this.HideWhen = value;
      }
    }

    public float Height
    {
      get
      {
        return _totalHeight;
      }
    }
    public BasePanel Owner { get; set; }
    public override float Width { get { return _width; } }

    float _width;
    float IParameter.Width
    {
      get
      {
        return _width;
      }

      set
      {
        _width = value;
      }
    }


    protected override void UpdateTotalHeight()
    {
      _totalHeight = WindowBoxStartY;
      _totalHeight += PaddingTop;
      _totalHeight += UpdateParametersCoordinates(Position.x, StartY + PaddingTop);
    }

    protected override float UpdateParametersCoordinates(float x, float y)
    {
      float totalHeight = 0;
      float columnWidth = Width / (float)_parameters.Count;
      {
        float max = 0;
        foreach (var param in _parameters)
        {
          if (param.Height > max)
          {
            max = param.Height;
          }
        }
        totalHeight = max;
      }

      foreach (var parameter in _parameters)
      {
        if (parameter.HideWhen())
        {
          continue;
        }
        parameter.Position = new Vec2(x + PaddingX, y);
        parameter.Width = columnWidth - PaddingX * 2;
        x += columnWidth;
      }

      return totalHeight;
    }

    public override void Draw()
    {
      UpdateEvents();

      UpdateTotalHeight();
      DrawWindowBox();
      DrawParameters();
    }

    public void Repaint()
    {
      Draw();
    }

    public void UpdateStyle()
    {
      foreach (var param in _parameters)
      {
        param.UpdateStyle();
      }
    }

    protected override void DrawWindowBox()
    {
    }
  }
}
