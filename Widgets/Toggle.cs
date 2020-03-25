namespace GUIPanels
{
  public class ToggleBase : HorizontalLayout
  {
    public Widget ToggleButton { get { return _filled; } }
    Col _primaryColor = Col.white, _secondaryColor = Col.black;
    public Col PrimaryColor
    {
      get { return _primaryColor; }
      set { _primaryColor = value; }
    }
    public Col SecondaryColor
    {
      get { return _secondaryColor; }
      set
      {
        _secondaryColor = value;
        _filled.Style.Set(Styles.BorderColor, _secondaryColor);
      }
    }
    public ToggleBase SetToggleSize(float size)
    {
      _initialized = false;
      _toggleSize = size;
      return this;
    }
    float _toggleSize = -1;
    protected EmptySpace _filled;
    Label _label;
    public ToggleBase(string title) : base()
    {
      // var horizontal = new HorizontalLayout();
      _filled = new EmptySpace();
      _filled.Style.Set(Styles.Border, new Dim(2));
      _filled.Style.Set(Styles.Margin, Dim.right * 5);
      /* horizontal. */
      Attach(_filled, _label = new Label(title));
      // AddChild(horizontal);

      Theme.Current.Apply(this, typeof(ToggleBase));
    }
    public ToggleBase SetLabelHidden(bool value = true)
    {
      _label.Style.SetHidden(value);
      return this;
    }

    bool _initialized = false;
    protected override void Render()
    {
      if (!_initialized)
      {
        if (_toggleSize > 0)
        {
          CurrentStyle.FontSize(_toggleSize);
          _toggleSize = -1;
        }
        _filled.Width = _filled.Height = Utils.CalcSize("a", CurrentStyle).y;
        _initialized = true;
      }
      base.Render();
    }
  }

  public class ToggleButton : ToggleBase
  {
    bool _isMouseDown;
    System.Action _action;
    public ToggleButton(string title, System.Action action = null) : base(title)
    {
      _action = action;
    }
    protected override void OnDraggingStart(float x, float y)
    {
      base.OnDraggingStart(x, y);
      _isMouseDown = true;
    }
    protected override void OnMouseUp()
    {
      _isMouseDown = false;
    }

    protected override void OnUpdate()
    {
      base.OnUpdate();
      _filled.Style.Set(Styles.BackgroundColor, _isMouseDown ? PrimaryColor : SecondaryColor);
    }
  }

  public class Toggle : ToggleBase
  {
    ValueComponent<bool> _value;
    public Toggle(string title, System.Func<bool> getValueCallback = null, System.Action<bool> setValueCallback = null) : base(title)
    {
      _value = new ValueComponent<bool>(getValueCallback, setValueCallback);
    }
    protected override void OnUpdate()
    {
      base.OnUpdate();
      _filled.Style.Set(Styles.BackgroundColor, _value.Value ? PrimaryColor : SecondaryColor);
    }
    protected override void OnClick()
    {
      base.OnClick();
      _value.Value = !_value.Value;
      _filled.Style.Set(Styles.BackgroundColor, _value.Value ? PrimaryColor : SecondaryColor);
    }
  }
}