namespace GUIPanels
{
  public class Label : DrawableComponent, IValueComponent<string>
  {
    public string Title { get; set; }
    public string Value
    {
      get { return _valueComponent.Value; }
      set { _valueComponent.Value = value; }
    }

    ValueComponent<string> _valueComponent;

    public Label(string title, System.Func<string> getValueCallback = null, System.Action<string> setValueCallback = null)
    {
      _valueComponent = new ValueComponent<string>(getValueCallback, setValueCallback);
      Title = title;
    }
    protected override float ContentHeight
    {
      get
      {
        return CalculatedTextSize.y + Style.Get<float>(Styles.LineHeight);
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
        if (value != null)
        {
          text = string.Format("{0}: {1}", Title, value);
        }
        return text;
      }
    }

    Vec2 CalculatedTextSize { get { return Utils.CalcSize(Text, Style); } }

    protected override void Render()
    {
      base.Render();
      Rendering.DrawText(new Rectangle(
        ContentPosition.x,
        ContentPosition.y,
        InnerWidth, Style.Get<float>(Styles.FontSize)), Text, Style);
    }
  }
}
