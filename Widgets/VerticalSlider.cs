using System;
namespace GUIPanels
{
  public class VerticalSlider : VerticalLayout
  {
    public Widget ActiveBar { get { return _activeBar; } }
    public Widget InactiveBar { get { return _inactiveBar; } }
    public Widget SliderBar { get { return _placeHolder; } }
    public float Width
    {
      get { return _width; }
      set
      {
        _width = value;
        _placeHolder.Style.Set<float>(Styles.Width, _width);
      }
    }
    public float Value
    {
      get { return _valueComponent.Value; }
      set
      {
        _valueComponent.Value = value;
        // value remap to 01
        float height = Utils.Map(value, _min, _max, 0, _height, true);
        _activeBar.Style.Set<float>(Styles.Height, height);
        _inactiveBar.Style.Set<float>(Styles.Height, _height - height);
      }
    }
    Widget _inactiveBar, _activeBar, _placeHolder;

    float _min, _max;
    float _width, _height;
    ValueComponent<float> _valueComponent;
    Label _valueLabel;
    public VerticalSlider(string title, Func<float> getValueCallback,
       Action<float> setValueCallback, float min = 0, float max = 1, float width = 12, float height = 50)
    {
      _min = min; _max = max; _width = width; _height = height;
      _valueComponent = new ValueComponent<float>(getValueCallback, setValueCallback);

      Attach(
        new HorizontalLayout().Attach(
          (_placeHolder = new VerticalLayout(_width)).Attach(
          (_inactiveBar = new EmptySpace(_width, _height)),
          (_activeBar = new EmptySpace(_width, 0))
          )
        ),
        (_valueLabel = new Label(title))
      );

      Value = getValueCallback();
    }
    bool _isDragging;
    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (_isDragging)
      {
        var pos = Utils.MousePosition();
        var box = _placeHolder.Box;
        var val = Utils.Map(pos.y - box.y, 0, _height, _max, _min, true);
        _valueComponent.Value = val;
      }
      Value = _valueComponent.Value;
    }
    protected override void OnDraggingStart(float x, float y)
    {
      base.OnDraggingStart(x, y);
      if (_placeHolder.Box.Contains(new Vec2(x, y)))
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
