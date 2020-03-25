namespace GUIPanels
{
  public class Label : Widget
  {
    public virtual string Title { get { return _getValueCallback == null ? _title : _getValueCallback(); } set { _title = value; } }

    string _title;

    System.Func<string> _getValueCallback;
    public Label(string title = "")
    {
      _title = title;
    }

    public Label SetValueCallback(System.Func<string> getValueCallback)
    {
      _getValueCallback = getValueCallback;
      return this;
    }

    public Label(System.Func<string> getValueCallback)
    {
      _getValueCallback = getValueCallback;
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
        return Title;
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
