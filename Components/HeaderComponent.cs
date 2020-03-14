namespace GUIPanels
{
  public class HeaderComponent : VerticalLayout
  {
    public bool IsCollapsed { get { return !_button.On; } }
    string _title;
    HorizontalGrid _layout;
    ToggleButton _button;
    public HeaderComponent(string title, System.Action<bool> onToggleClicked) : base(100)
    {
      _title = title;
      Style.Set(Styles.BackgroundColor, new Col(0.5f, 0.5f, 0.5f, 1));
      Style.Set(Styles.Height, 12f);
      Style.Set(Styles.Padding, Dim.bottom * 1 + Dim.top * 2);
      Style.Set(Styles.Margin, Dim.bottom * 4);
      // 
      var layout = _layout = new HorizontalGrid();
      var hL0 = new HorizontalLayout();
      var hL1 = new HorizontalLayout(true);

      hL0.AddChild(new Label(title));
      _button = new ToggleButton(true, onToggleClicked);
      // add toggle button to hl1
      hL1.AddChild(_button);

      layout.AddChild(hL0);
      layout.AddChild(hL1);
      AddChild(layout);
    }
  }
}
