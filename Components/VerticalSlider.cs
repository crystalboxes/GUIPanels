using System;
namespace GUIPanels
{
  public sealed class VerticalSlider : HorizontalLayout
  {
    public EmptySpace ActiveBar { get { return _filledSpace; } }
    public EmptySpace InactiveBar { get { return _emptySpace; } }

    public Label Label { get { return _label; } }

    ValueComponent<float> _valueComponent;
    EmptySpace _emptySpace, _filledSpace;
    VerticalLayout _verticalLayout;
    float _min, _max;
    public float Width
    {
      get { return Style.Get<float>(Styles.Width); }
      set { Style.Set<float>(Styles.Width, value); }
    }
    public float Height
    {
      get { return Style.Get<float>(Styles.Width); }
      set { Style.Set<float>(Styles.Width, value); }
    }
    Label _label;

    float Value
    {
      get { return _valueComponent.Value; }
      set
      {
        _valueComponent.Value = value;
        var val01 = Utils.Map(value, _min, _max, 0, 1, true);
        var empty01 = 1 - val01;
        _emptySpace.Style.Set(Styles.Height, empty01 * Height);
        _filledSpace.Style.Set(Styles.Height, val01 * Height);
      }
    }
    public VerticalSlider(string title, Func<float> getValueCallback, Action<float> setValueCallback,
      float min = 0, float max = 1, float width = 10f, float height = 50) : base()
    {
      _min = min;
      _max = max;
      Width = width;
      Height = height;
      _valueComponent = new ValueComponent<float>(getValueCallback, setValueCallback);
      _verticalLayout = new VerticalLayout(width);
      _emptySpace = new EmptySpace(width, 0);
      _filledSpace = new EmptySpace(width, 0);
      _emptySpace.Style.Set(Styles.BackgroundColor, Col.black);
      _filledSpace.Style.Set(Styles.BackgroundColor, Col.white);
      _verticalLayout.Attach(_emptySpace, _filledSpace);
      _label = new ValueLabel(title, () =>
      {
        var str = string.Format("{0:0.00}", Value);
        float w = Utils.CalcSize(str, Style).x;
        if (w > Width * 0.5f)
        {
          return "";
        }
        return str;
      });
      _verticalLayout.Attach(_label);
      AddChild(_verticalLayout);
      Value = getValueCallback();
    }
    bool _isDragging;
    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (_isDragging)
      {
        var pos = Utils.MousePosition();
        var box = _verticalLayout.Box;
        var val = Utils.Map(pos.y - box.y, 0, Height, _max, _min, true);
        _valueComponent.Value = val;
      }
      Value = _valueComponent.Value;
    }
    protected override void OnDraggingStart(float x, float y)
    {
      base.OnDraggingStart(x, y);
      if (_verticalLayout.Box.Contains(new Vec2(x, y)))
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
