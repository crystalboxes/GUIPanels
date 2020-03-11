using System;

namespace GUIPanels
{
  public class ParameterGroup : BasePanelCollapsible, IParameter
  {
    public static PanelSettings DefaultSettings
    {
      get
      {
        var s = new PanelSettings();
        s.HeaderHeight = 15f;
        s.HeaderOffset = 1f;
        s.HeaderColor = Col.black;
        s.PaddingX = 3;
        s.PaddingY = 10f;
        s.PaddingTop = 4f;
        s.HeaderTextColor = Col.white;
        return s;
      }
    }

    public ParameterGroup(PanelSettings settings, string name = "") : base(settings)
    {
      Name = name;
    }

    public ParameterGroup(string name = "") : base(DefaultSettings)
    {
      Name = name;
    }

    public override float Width
    {
      get { return Owner.Width; }
    }

    public float Height
    {
      get { return _totalHeight; }
    }

    Vec2 _pos;

    public override Vec2 Position
    {
      get { return new Vec2(Owner.Position.x, _pos.y); }
      set { _pos = value; }
    }

    public BasePanel Owner { get; set; }

    protected override float HeaderTextSize
    {
      get { return HeaderHeight * 0.5f; }
    }

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


    float IParameter.Width
    {
      get { return this.Width; }
      set { }
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