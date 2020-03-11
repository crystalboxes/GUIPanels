using System;

namespace GUIPanels
{
  public class VerticalLayoutOld : BasePanel, IParameter
  {
    public static BasePanelSettings DefaultSettings
    {
      get
      {
        var s = new BasePanelSettings();
        s.PaddingX = 3;
        s.PaddingY = 10f;
        s.PaddingTop = 4f;
        return s;
      }
    }

    public VerticalLayoutOld(BasePanelSettings settings, string name = "") : base(settings, name)
    {
    }

    public VerticalLayoutOld(string name = "") : base(DefaultSettings, name)
    {
    }

    float _width;
    float IParameter.Width
    {
      get { return _width; }
      set { _width = value; }
    }

    public override float Width
    {
      get { return _width; }
    }

    public float Height
    {
      get { return _totalHeight; }
    }

    Vec2 _pos;

    public override Vec2 Position
    {
      get { return _pos; }
      set { _pos = value; }
    }

    public BasePanel Owner { get; set; }


    public override Style Style
    {
      get
      {
        return Owner == null ? null : Owner.Style;
      }
      set { }
    }

    public override float TextSize
    {
      get { return Owner.TextSize; }
    }

    public override Col TextColor
    {
      get { return Owner.TextColor; }
    }

    public override float PaddingLine
    {
      get { return Owner.PaddingLine; }
    }



    public void Repaint()
    {
      base.Draw();
    }

    public void UpdateStyle()
    {
      foreach (var param in _parameters)
      {
        param.UpdateStyle();
      }
    }

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

    public System.Func<bool> HideWhen = () => false;
    protected override void DrawWindowBox()
    {
    }
  }
}