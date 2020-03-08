using System;

namespace GUIPanels
{
  public class ParameterGroup : BasePanelCollapsable, IParameter
  {
    string _name;

    protected override string Name
    {
      get { return _name; }
    }

    public ParameterGroup(string name = "")
    {
      _name = name;
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

    public override Style Style
    {
      get { return Owner.Style; }
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

    protected override float HeaderHeight
    {
      get { return 15f; }
    }

    protected override float HeaderOffset
    {
      get { return 1f; }
    }

    protected override float PaddingX
    {
      get { return 3; }
    }

    protected override float PaddingY
    {
      get { return 10f; }
    }

    protected override float PaddingTop
    {
      get { return 4f; }
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

    protected override Col HeaderTextColor
    {
      get { return Col.white; }
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