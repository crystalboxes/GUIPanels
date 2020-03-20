namespace GUIPanels
{
  public class HeaderWidget : VerticalLayout
  {
    public bool IsCollapsed { get { return !_button.On; } }
    ToggleButton _button;
    public HeaderWidget(string title, System.Action<bool> onToggleClicked) : base(100)
    {
      var layout = new HorizontalGrid();
      var hL0 = new HorizontalLayout();
      var hL1 = new HorizontalLayout(true);

      hL0.Attach(new Label(title));
      _button = new ToggleButton(true, onToggleClicked);
      // add toggle button to hl1
      hL1.Attach(_button);

      layout.Attach(hL0, hL1);
      AddChild(layout);
    }
  }
}
