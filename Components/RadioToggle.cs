namespace GUIPanels
{
  public class RadioToggle : HorizontalGrid
  {
    ValueComponent<bool> _value;
    EmptySpace _filled;
    public RadioToggle(string title, System.Func<bool> getValueCallback = null, System.Action<bool> setValueCallback = null)
    {
      float height = Utils.CalcSize("a", Style).y;
      _value = new ValueComponent<bool>(getValueCallback, setValueCallback);

      var horizontal = new HorizontalLayout();

      _filled = new EmptySpace(height, height);
      // _filled.Style.Set(Styles.BackgroundColor, Col.red);

      horizontal.AddChild(_filled);
      horizontal.AddChild(new Label(title));
      _filled.Style.Set(Styles.Margin, Dim.right * 5);

      AddChild(horizontal);
    }
    protected override void OnClick()
    {
      base.OnClick();
      _value.Value = !_value.Value;
    }
    protected override void Render()
    {
      base.Render();
      var box = _filled.ContentBox;
      float w = _filled.Style.Get<float>(Styles.Width);
      float radius = w * 0.5f;

      var center = new Vec2(box.x + radius, box.y + radius);
      Rendering.DrawCircle(center, radius, Col.black);
      if (_value.Value)
      {
        Rendering.DrawCircle(center, radius * 0.5f, Col.white);
      }
    }
  }
}