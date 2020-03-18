namespace GUIPanels
{
  public class HeaderComponent : VerticalLayout
  {
    public bool IsCollapsed { get { return !_button.On; } }
    ToggleButton _button;
    public HeaderComponent(string title, System.Action<bool> onToggleClicked) : base(100)
    {
      var layout = new HorizontalGrid();
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
