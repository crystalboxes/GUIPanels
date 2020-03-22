namespace GUIPanels
{
  public abstract class HeaderBase : VerticalLayout
  {
    public abstract bool IsCollapsed { get; }
    public HeaderBase(string title) : base(100)
    {
    }

    public abstract void SetToggleCallback(System.Action<bool> action);

  }
  public class HeaderWidget2 : HeaderBase
  {
    public class ToggleButton : Label
    {
      public bool On { get { return _isOn; } }
      public System.Action<bool> OnToggleClicked = null;
      bool _isOn;
      public ToggleButton(bool isOn) : base("[+]")
      {
        Style.Set(Styles.Width, 12f);
        _isOn = isOn;
        UpdateTitle();
      }

      protected override void OnClick()
      {
        base.OnClick();
        // UnityEngine.Debug.Log("hey");
        _isOn = !_isOn;
        if (OnToggleClicked != null)
        {
          OnToggleClicked(_isOn);
        }
        UpdateTitle();
      }
      void UpdateTitle()
      {
        Title = _isOn ? "[-]" : "[+]";
      }
    }
    public override bool IsCollapsed { get { return !_button.On; } }
    ToggleButton _button;
    public HeaderWidget2(string title) : base(title)
    {
      var layout = new HorizontalGrid();
      var hL0 = new HorizontalLayout();
      var hL1 = new HorizontalLayout(true);

      hL0.Attach(new Label(title));
      _button = new ToggleButton(true);
      // add toggle button to hl1
      hL1.Attach(_button);

      layout.Attach(hL0, hL1);
      AddChild(layout);
    }
    public override void SetToggleCallback(System.Action<bool> action)
    {
      _button.OnToggleClicked = action;
    }
  }
  public class HeaderWidget : HeaderBase
  {
    bool _on = true;
    public override bool IsCollapsed { get { return !_on; } }
    public HeaderWidget(string title) : base(title)
    {
      var layout = new HorizontalGrid();
      var hL0 = new HorizontalLayout();

      hL0.Attach(new Label(title));
      layout.Attach(hL0);
      AddChild(layout);
    }

    public override void SetToggleCallback(System.Action<bool> action)
    {
      AddEventListener(EventType.DoubleClick, () =>
      {
        _on = !_on;
        action(_on);
      });
    }


  }
}
