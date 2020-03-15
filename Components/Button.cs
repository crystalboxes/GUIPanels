namespace GUIPanels
{
  public class Button : VerticalLayout
  {
    Label _label;
    System.Action _onClickCallback;
    public Button(string title, System.Action onClickCallback = null) : base()
    {
      _onClickCallback = onClickCallback;
      _label = new Label(title);
      _label.Style.Set<Col>(Styles.FontColor, Col.black);
      Style.Set<Col>(Styles.BackgroundColor, Col.white);
      var size = Utils.CalcSize(_label.Title, _label.Style);
      AddChild(new EmptySpace(0, size.y));
    }
    protected override void OnClick()
    {
      base.OnClick();
      _onClickCallback();
    }
    protected override void OnDraggingStart(float x, float y)
    {
      base.OnDraggingStart(x, y);
      _label.Style.Set<Col>(Styles.FontColor, Col.white);
      Style.Set<Col>(Styles.BackgroundColor, Col.black);
    }
    protected override void OnMouseUp()
    {
      base.OnMouseUp();
      _label.Style.Set<Col>(Styles.FontColor, Col.black);
      Style.Set<Col>(Styles.BackgroundColor, Col.white);
    }
    protected override void Render()
    {
      base.Render();
      // Draw text horizontally aligned.
      var pos = ContentPosition;
      var size = Utils.CalcSize(_label.Title, _label.Style);
      var center = new Vec2(pos.x + InnerWidth * 0.5f,
        pos.y + InnerHeight * 0.5f);
      var textP = new Vec2(center.x - size.x * 0.5f, center.y - size.y * 0.5f);
      _label.Position = textP;
      _label.Draw();
    }
  }
}
