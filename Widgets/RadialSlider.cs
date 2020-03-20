using System;

namespace GUIPanels
{
  public abstract class RadialSlider : VerticalLayout
  {
    public float Value
    {
      get { return _valueComponent.Value; }
      set { _valueComponent.Value = value; }
    }

    public float Radius
    {
      get { return _radius; }
      set
      {
        _radius = value;
        _emptySpace.Style.Set<float>(Styles.Height, _radius * 2);
      }
    }

    public Col EmptyColor { get; set; }
    public Col FilledColor { get; set; }
    protected float MinRange { get { return _min; } }
    protected float MaxRange { get { return _max; } }
    protected Vec2 Center
    {
      get
      {
        return new Vec2(ContentPosition.x + Radius, ContentPosition.y + Radius);
      }
    }
    EmptySpace _emptySpace;
    ValueComponent<float> _valueComponent;
    float _radius;
    float _min, _max;
    protected Rectangle RadialBox { get { return _emptySpace.Box; } }

    public RadialSlider(string title, Func<float> getValueCallback, Action<float> setValueCallback,
      float min = 0, float max = 1, float radius = 25f) : base(radius)
    {
      _radius = radius;
      _min = min;
      _max = max;

      _emptySpace = new EmptySpace(_radius * 2, _radius * 2);
      AddChild(_emptySpace);
      AddChild(new ValueLabel(title, () => string.Format("{0:0.00}", Value)));

      _valueComponent = new ValueComponent<float>(getValueCallback, setValueCallback);
    }
    bool _isDragging; Vec2 _dragStart; float _startVal;
    protected bool IsDragging { get { return _isDragging; } }
    protected Vec2 DragStart { get { return _dragStart; } }
    protected float StartVal { get { return _startVal; } }

    protected override void OnDraggingStart(float x, float y)
    {
      base.OnDraggingStart(x, y);
      var pos = new Vec2(x, y);
      var box = RadialBox;
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

    const float _offsetRate = 0.01f;
    protected virtual void ValueUpdate()
    {
      var pos = Utils.MousePosition();
      var box = RadialBox;
      var offset = DragStart - pos;
      Value = Utils.Clamp(StartVal + offset.y * _offsetRate, MinRange, MaxRange);
    }

    protected abstract void DrawSlider();

    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (IsDragging)
      {
        ValueUpdate();
      }
    }
    protected override void Render()
    {
      base.Render();
      DrawSlider();
    }
  }


  public class CircleSlider : RadialSlider
  {
    public CircleSlider(string title, Func<float> getValueCallback, Action<float> setValueCallback,
      float min = 0, float max = 1, float radius = 25f)
      : base(title, getValueCallback, setValueCallback, min, max, radius)
    {
    }
    protected override void DrawSlider()
    {
      var box = RadialBox;
      var center = Center;
      Rendering.DrawCircle(center, Radius, EmptyColor);
      Rendering.DrawCircle(center, Radius * Utils.Map(Value, MinRange, MaxRange, 0, 1), FilledColor);
    }
  }

  public class RotarySlider : RadialSlider
  {
    bool _radialValueUpdate;
    public float Width { get; set; }
    public RotarySlider(string title, Func<float> getValueCallback, Action<float> setValueCallback,
      float min = 0, float max = 1, float radius = 25f, float width = 9f)
      : base(title, getValueCallback, setValueCallback, min, max, radius)
    {
      Width = width;
    }
    public RotarySlider SetRadialUpdateValueType(bool radialValueUpdate = true)
    {
      _radialValueUpdate = radialValueUpdate;
      return this;
    }

    protected override void ValueUpdate()
    {
      if (_radialValueUpdate)
      {
        // TODO Remove unityengine code
        var mpos = Utils.MousePosition();
        UnityEngine.Vector2 pos = Utils.MousePosition() - Center;
        pos.Normalize();
        // get 
        var zeroOne = new UnityEngine.Vector2(0, 1);
        float angle01 = 360 - UnityEngine.Vector2.Angle(zeroOne, pos);
        if (mpos.x < Center.x)
        {
          angle01 = 180 - (angle01 - 180);
        }
        Value = Utils.Map(angle01, 0, 360, MinRange, MaxRange);
      }
      else
      {
        base.ValueUpdate();
      }
    }

    protected override void DrawSlider()
    {
      var box = RadialBox;
      var center = Center;
      // DrawCircle
      Rendering.DrawArcRing(center, Radius, Radius - Width, 0, 360, EmptyColor);
      Rendering.DrawArcRing(center,
        Radius, Radius - Width, (1 - Utils.Map(Value, MinRange, MaxRange, 0, 1)) * 360,
        359, FilledColor);
    }
  }
}
