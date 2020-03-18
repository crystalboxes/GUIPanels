namespace GUIPanels
{
  public class Label : Widget
  {
    public virtual string Title { get; set; }
    public virtual string Value
    {
      get { return Title; }
      set { Title = value; }
    }

    public Label(string title)
    {
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
        return Title;
      }
    }

    Vec2 CalculatedTextSize { get { return Utils.CalcSize(Text, Style); } }

    protected override void Render()
    {
      Rendering.DrawText(new Rectangle(
        ContentPosition.x,
        ContentPosition.y,
        InnerWidth, Style.Get<float>(Styles.FontSize)), Text, Style);
    }
  }
}
