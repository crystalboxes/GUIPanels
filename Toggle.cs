using System;

namespace GUIPanels
{
  public class Toggle : Parameter
  {
    public override float Height
    {
      get { return Owner.TextSize; }
    }
    string _title;
    bool _value;
    Action<bool> _callback;
    public Toggle(string title, Action<bool> cb, bool value)
    {
      _title = title;
      _value = value;
      _callback = cb;
    }

    Rectangle ToggleBoxRect
    {
      get
      {
        return new Rectangle(Position.x, Position.y, Height, Height);
      }
    }

    Rectangle ToggleCheckedBoxRect
    {
      get
      {
        var r = ToggleBoxRect;
        float w = r.width * CheckedRectScale;
        r.x += (r.width - w) / 2f;
        r.y += (r.width - w) / 2f;
        r.width = r.height = w;
        return r;
      }
    }

    float TextOffset { get { return 5f; } }
    float CheckedRectScale { get { return 0.5f; } }

    Col PrimaryColor { get { return Col.black; } }
    Col SecondaryColor { get { return Col.white; } }

    public override void Repaint()
    {
      var r = ToggleBoxRect;

      if (ToggleBoxRect.Contains(Utils.MousePosition()) && Owner.MouseUp)
      {
        _value = !_value;
      }


      // draw box and then text
      Renderer.Current.DrawRect(r, PrimaryColor);

      if (_value)
      {
        Renderer.Current.DrawRect(ToggleCheckedBoxRect, SecondaryColor);
      }
      r.x += TextOffset + r.width;
      r.width = Width - r.width;
      Renderer.Current.DrawText(r, _title, Owner.Style);
    }

    public override void UpdateStyle()
    {
    }
  }
}