namespace GUIPanels
{
  public class RadioToggle : HorizontalGrid
  {
    public EmptySpace RadioIcon { get { return _filled; } }
    public Col PrimaryColor = Col.black;
    public Col SecondaryColor = Col.white;
    public float CheckedRadiusRatio = 0.5f;

    ValueComponent<bool> _value;
    EmptySpace _filled;

    public RadioToggle(string title, System.Func<bool> getValueCallback = null, System.Action<bool> setValueCallback = null)
    {
      var horizontal = new HorizontalLayout();
      _value = new ValueComponent<bool>(getValueCallback, setValueCallback);
      _filled = new EmptySpace();
      _filled.Style.Set(Styles.Margin, Dim.right * 5);
      horizontal.Attach(_filled, new Label(title));
      AddChild(horizontal);
    }

    protected override void OnClick()
    {
      base.OnClick();
      _value.Value = !_value.Value;
    }

    bool _initialized = false;
    protected override void Render()
    {
      if (!_initialized)
      {
        _filled.Width = _filled.Height = Utils.CalcSize("a", CurrentStyle).y;
        _initialized = true;
      }
      base.Render();
      var box = _filled.ContentBox;
      float w = _filled.CurrentStyle.Get<float>(Styles.Width);
      float radius = w * 0.5f;

      var center = new Vec2(box.x + radius, box.y + radius);
      Rendering.DrawCircle(center, radius, PrimaryColor);
      if (_value.Value)
      {
        Rendering.DrawCircle(center, radius * CheckedRadiusRatio, SecondaryColor);
      }
    }
  }
}
