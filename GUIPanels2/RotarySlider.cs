using System;

namespace GUIPanels
{
  public class RotarySlider : VerticalLayout
  {
    ValueComponent<float> _valueComponent;
    public float Value
    {
      get { return _valueComponent.Value; }
      set { _valueComponent.Value = value; }
    }

    public float Radius
    {
      get
      {
        return _radius;
      }
      set
      {
        _radius = value;
        _emptySpace.Style.Set<float>(Styles.Height, _radius * 2);
      }
    }

    float _radius;
    public float Width { get; set; }
    public Col EmptyColor { get; set; }
    public Col FilledColor { get; set; }
    EmptySpace _emptySpace;
    float _min, _max;
    public RotarySlider(string title, Func<float> getValueCallback, Action<float> setValueCallback, float min = 0, float max = 1, float radius = 25f, float width = 9f) : base(radius)
    {
      _radius = radius;
      Width = width;
      _min = min;
      _max = max;

      _emptySpace = new EmptySpace(_radius * 2, _radius * 2);
      AddChild(_emptySpace);
      AddChild(new Label(title, () => string.Format("{0:0.00}", Value)));

      _valueComponent = new ValueComponent<float>(getValueCallback, setValueCallback);

      EmptyColor = Col.black;
      FilledColor = Col.white;
    }

    protected override void Render()
    {
      base.Render();
      var box = _emptySpace.Box;
      var center = new Vec2(ContentPosition.x + Radius, ContentPosition.y + Radius);
      // Draw circle
      Rendering.DrawArcRing(center, Radius, Radius - Width, 0, 360, EmptyColor);
      Rendering.DrawArcRing(center, Radius, Radius - Width, (1 - Utils.Map(Value, _min, _max, 0, 1)) * 360, 359, FilledColor);
    }

    bool _isDragging; Vec2 _dragStart; float _startVal;
    const float _offsetRate = 0.01f;
    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (_isDragging)
      {
        var pos = Utils.MousePosition();
        var box = _emptySpace.Box;
        var offset = _dragStart - pos;
        _valueComponent.Value = Utils.Clamp(_startVal + offset.y * _offsetRate, _min, _max);
      }
      Value = _valueComponent.Value;
    }
    protected override void OnDraggingStart(float x, float y)
    {
      base.OnDraggingStart(x, y);
      var pos = new Vec2(x, y);
      var box = _emptySpace.Box;
      if (box.Contains(pos))
      {
        _isDragging = true;
        _dragStart = pos;
        _startVal = Value;
      }
    }
    protected override void OnMouseUp()
    {
      base.OnMouseUp();
      _isDragging = false;
    }
  }
}