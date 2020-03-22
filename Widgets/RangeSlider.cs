using System;

namespace GUIPanels
{
  public class RangeSlider : VerticalLayout
  {
    public HorizontalGrid InactiveBar { get { return _inactiveBar; } }
    public EmptySpace ActiveBar { get { return _activeBar; } }
    HorizontalGrid _inactiveBar;
    EmptySpace _activeBar, _placeHolder;
    ValueComponent<Vec2> _valueComponent;
    float _min, _max;
    Vec2 Value { get { return _valueComponent.Value; } set { _valueComponent.Value = value; } }
    float _height;
    float Height
    {
      get
      {
        return _height;
      }
      set
      {
        _height = Height;
        _placeHolder.Style.Set<float>(Styles.Height, _height);
        _inactiveBar.Style.Set<float>(Styles.Height, _height);
        _activeBar.Style.Set<float>(Styles.Height, _height);
      }
    }
    public RangeSlider SetHeight(float height)
    {
      Height = height;
      return this;
    }

    bool _initialized;
    ValueLabel _valueLabel;
    bool _isLabelUnderSlider;
    public void BringLabelToBottom(bool value = true)
    {
      _isLabelUnderSlider = value;
      BuildLayout();
    }
    void BuildLayout()
    {
      Children.Clear();
      if (_isLabelUnderSlider)
      {
        Attach(_inactiveBar, _valueLabel);
      }
      else
      {
        Attach(_valueLabel, _inactiveBar);
      }
    }

    public RangeSlider(string title, Func<Vec2> getValueCallback, Action<Vec2> setValueCallback, float min, float max) : base()
    {
      _min = min;
      _max = max;
      _height = 10;
      _valueComponent = new ValueComponent<Vec2>(getValueCallback, setValueCallback);
      _inactiveBar = (HorizontalGrid)new HorizontalGrid().Attach(_placeHolder = new EmptySpace(_height, _height));
      _valueLabel = new ValueLabel(title, () => string.Format("[{0:0.00}:{1:0.00}]", Value.x, Value.y));
      BuildLayout();

      _activeBar = new EmptySpace(10, Height);
      SetParent(_activeBar);
    }
    protected override void Render()
    {
      base.Render();
      if (!_initialized)
      {
        _activeBar.Style.Set(Styles.Width, (RangeEndX01 - RangeStartX01) * InnerWidth);
        _initialized = true;
      }
      var pos = _placeHolder.Position;
      _activeBar.Position = new Vec2(pos.x + Utils.Map(Value.x, _min, _max, 0, 1) * InnerWidth, pos.y);
      _activeBar.Draw();
    }

    bool _isDragging;
    float GetPos01(float x)
    {
      return Utils.Clamp01((x - ContentPosition.x) / ContentBox.width);
    }
    float RangeStartX01
    {
      get { return Utils.Map(Value.x, _min, _max, 0, 1, true); }
      set { Value = new Vec2(Utils.Map(value, 0, 1, _min, _max), Value.y); }
    }
    float RangeEndX01
    {
      get { return Utils.Map(Value.y, _min, _max, 0, 1, true); }
      set { Value = new Vec2(Value.x, Utils.Map(value, 0, 1, _min, _max)); }
    }
    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (_isDragging)
      {
        var px01 = GetPos01(Utils.MousePosition().x);
        var delta = px01 - startX01;

        if (_adjustmentState == SliderAdjustmentState.Left)
        {
          RangeStartX01 = px01;
        }
        else if (_adjustmentState == SliderAdjustmentState.Move)
        {
          RangeStartX01 = Utils.Clamp01(RangeStartX01 + delta);
          RangeEndX01 = Utils.Clamp01(RangeEndX01 + delta);
          startX01 = px01;
        }
        else if (_adjustmentState == SliderAdjustmentState.Right)
        {
          RangeEndX01 = px01;
        }
        _activeBar.Style.Set(Styles.Width, (RangeEndX01 - RangeStartX01) * InnerWidth);
      }
    }

    float startX01;
    enum SliderAdjustmentState
    {
      Move, Left, Right
    }
    SliderAdjustmentState _adjustmentState;
    protected override void OnDraggingStart(float x, float y)
    {
      base.OnDraggingStart(x, y);
      if (_isDragging)
      {
        return;
      }
      _isDragging = true;
      var box = ContentBox;
      var pos = Utils.MousePosition() - box.Position;
      startX01 = pos.x / box.width;

      if (startX01 < RangeStartX01)
      {
        _adjustmentState = SliderAdjustmentState.Left;
      }
      else if (startX01 > RangeEndX01)
      {
        _adjustmentState = SliderAdjustmentState.Right;
      }
      else
      {
        _adjustmentState = SliderAdjustmentState.Move;
      }
    }

    protected override void OnMouseUp()
    {
      base.OnMouseUp();
      _isDragging = false;
    }
  }
}
