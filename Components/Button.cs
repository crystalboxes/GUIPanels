namespace GUIPanels
{
  public class Button : VerticalLayout
  {
    public Label Label { get { return _label; } }
    public Col ClickedColor
    {
      get { return _clicked ? _secondaryColor : _primaryColor; }
      set
      {
        _primaryColor = value;
        Label.Style.Set<Col>(Styles.FontColor, value);
      }
    }

    public Col ButtonColor
    {
      get { return _clicked ? _primaryColor : _secondaryColor; }
      set
      {
        _secondaryColor = value;
        Style.Set<Col>(Styles.BackgroundColor, value);
      }
    }

    Col _primaryColor = Col.black,
      _secondaryColor = Col.white;

    Label _label;
    System.Action _onClickCallback;
    bool _clicked;

    public Button(string title, System.Action onClickCallback = null) : base()
    {
      _onClickCallback = onClickCallback;
      _label = new Label(title);
      var size = Utils.CalcSize(_label.Title, _label.Style);
      AddChild(new EmptySpace(0, size.y));
    }

    protected override void OnDraggingStart(float x, float y)
    {
      base.OnDraggingStart(x, y);
      _clicked = true;
      Label.Style.Set<Col>(Styles.FontColor, ClickedColor);
      Style.Set<Col>(Styles.BackgroundColor, ButtonColor);
    }
    protected override void OnMouseUp()
    {
      base.OnMouseUp();
      if (_clicked)
      {
        _onClickCallback();
      }
      _clicked = false;
      Label.Style.Set<Col>(Styles.FontColor, ClickedColor);
      Style.Set<Col>(Styles.BackgroundColor, ButtonColor);
    }

    protected override void Render()
    {
      base.Render();
      // Draw text horizontally aligned.
      var pos = ContentPosition;
      var size = Utils.CalcSize(Label.Title, Label.Style);
      var center = new Vec2(pos.x + InnerWidth * 0.5f,
        pos.y + InnerHeight * 0.5f);
      var textP = new Vec2(center.x - size.x * 0.5f, center.y - size.y * 0.5f);
      Label.Position = textP;
      Label.Draw();
    }
  }
}
