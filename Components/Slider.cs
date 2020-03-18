namespace GUIPanels
{
  public class Slider : VerticalLayout
  {
    public EmptySpace ActiveBar { get { return _activeBar; } }
    public HorizontalGrid InactiveBar { get { return _inactiveBar; } }
    public float SliderHeight
    {
      get { return _activeBar.Style.Get<float>(Styles.Height); }
      set { _activeBar.Style.Set(Styles.Height, value); }
    }
    Label _label;
    EmptySpace _activeBar;
    HorizontalGrid _inactiveBar;
    float Value
    {
      get { return _valueComponent.Value; }
      set
      {
        _valueComponent.Value = value;
        // map value into range
        var val = Utils.Map(value, _min, _max, 0, InnerWidth, true);
        _activeBar.Style.Set(Styles.Width, val);
      }
    }

    float _min, _max;

    public Slider(string title, System.Func<float> getValueCallback = null, System.Action<float> setValueCallback = null, float min = 0, float max = 1, float width = 100f) : base(width)
    {
      _valueComponent = new ValueComponent<float>(getValueCallback, setValueCallback);
      AddChild(new ValueLabel(title, () => string.Format("{0:0.00}", Value)));
      _max = max;
      _min = min;
      var verticalLayout = new VerticalLayout();
      _inactiveBar = new HorizontalGrid();
      var horizontalLayout = new HorizontalLayout();
      _activeBar = new EmptySpace();
      _inactiveBar.Attach(horizontalLayout);
      verticalLayout.Attach(_inactiveBar);
      horizontalLayout.Attach(_activeBar);

      Value = getValueCallback();
      AddChild(verticalLayout);
    }
    ValueComponent<float> _valueComponent;
    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (_isDragging)
      {
        var pos = Utils.MousePosition();
        var box = _inactiveBar.Box;
        var val = Utils.Map(pos.x - box.x, 0, box.width, _min, _max, true);
        _valueComponent.Value = val;
      }
      Value = _valueComponent.Value;
    }
    bool _isDragging;
    protected override void OnDraggingStart(float x, float y)
    {
      base.OnDraggingStart(x, y);
      if (_inactiveBar.Box.Contains(new Vec2(x, y)))
      {
        _isDragging = true;
      }
    }
    protected override void OnMouseUp()
    {
      base.OnMouseUp();
      _isDragging = false;
    }
  }
}
