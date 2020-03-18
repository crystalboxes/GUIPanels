namespace GUIPanels
{
  public class Toggle : HorizontalGrid
  {
    ValueComponent<bool> _value;
    EmptySpace _filled;
    public Toggle(string title, System.Func<bool> getValueCallback = null, System.Action<bool> setValueCallback = null)
    {
      float height = Utils.CalcSize("a", Style).y;
      _value = new ValueComponent<bool>(getValueCallback, setValueCallback);
      var horizontal = new HorizontalLayout();
      _filled = new EmptySpace(height, height);
      _filled.Style.Set(Styles.Border, new Dim(2));
      _filled.Style.Set(Styles.BorderColor, Col.black);
      _filled.Style.Set(Styles.BackgroundColor, _value.Value ? Col.white : Col.black);
      horizontal.Attach(_filled, new Label(title));
      _filled.Style.Set(Styles.Margin, Dim.right * 5);
      AddChild(horizontal);
    }
    protected override void OnUpdate()
    {
      base.OnUpdate();
      _filled.Style.Set(Styles.BackgroundColor, _value.Value ? Col.white : Col.black);
    }
    protected override void OnClick()
    {
      base.OnClick();
      _value.Value = !_value.Value;
      _filled.Style.Set(Styles.BackgroundColor, _value.Value ? Col.white : Col.black);
    }
  }
}