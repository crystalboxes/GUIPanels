namespace GUIPanels
{
  public class ValueLabel : Label, IValueComponent<string>
  {
    public override string Title { get; set; }
    public override string Value
    {
      get { return _valueComponent.Value; }
      set { _valueComponent.Value = value; }
    }

    ValueComponent<string> _valueComponent;

    public ValueLabel(string title, System.Func<string> getValueCallback = null, System.Action<string> setValueCallback = null) : base(title)
    {
      _valueComponent = new ValueComponent<string>(getValueCallback, setValueCallback);
      Title = title;
    }
    protected override float ContentHeight
    {
      get
      {
        return CalculatedTextSize.y + CurrentStyle.Get<float>(Styles.LineHeight);
      }
    }
    protected override float ContentWidth
    {
      get
      {
        return CalculatedTextSize.x;
      }
    }

    string Text
    {
      get
      {
        var value = Value;
        var text = Title;
        if (value != null && value.Length > 0)
        {
          text = string.Format("{0}: {1}", Title, value);
        }
        return text;
      }
    }

    Vec2 CalculatedTextSize { get { return Utils.CalcSize(Text, CurrentStyle); } }

    protected override void Render()
    {
      Rendering.DrawText(new Rectangle(
        ContentPosition.x,
        ContentPosition.y,
        InnerWidth, CurrentStyle.Get<float>(Styles.FontSize)), Text, CurrentStyle);
    }
  }
}
