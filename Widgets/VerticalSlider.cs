using System;
namespace GUIPanels
{
  public class VerticalSlider : VerticalLayout
  {
    public Widget ActiveBar { get { return _activeBar; } }
    public Widget InactiveBar { get { return _inactiveBar; } }
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

      _placeHolder = new EmptySpace(_width, _height);
      _inactiveBar = new EmptySpace(_width, _height);
      _activeBar = new EmptySpace(_width, 0);

      _valueLabel = new Label(title);
      Attach(
        _placeHolder,
        _valueLabel
      );

      Value = getValueCallback();
    }
    protected override void Render()
    {
      base.Render();
      _inactiveBar.Position = _placeHolder.Position;
      _activeBar.Position = _placeHolder.Position + new Vec2(0, _inactiveBar.Box.height);
      _inactiveBar.Draw();
      _activeBar.Draw();
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

  // public sealed class VerticalSlider2 : HorizontalLayout
  // {
  //   public EmptySpace ActiveBar { get { return _filledSpace; } }
  //   public EmptySpace InactiveBar { get { return _emptySpace; } }

  //   public Label Label { get { return _label; } }

  //   ValueComponent<float> _valueComponent;
  //   EmptySpace _emptySpace, _filledSpace;
  //   VerticalLayout _verticalLayout;
  //   float _min, _max;
  //   public float Width
  //   {
  //     get { return Style.Get<float>(Styles.Width); }
  //     set { Style.Set<float>(Styles.Width, value); }
  //   }
  //   public float Height
  //   {
  //     get { return Style.Get<float>(Styles.Width); }
  //     set { Style.Set<float>(Styles.Width, value); }
  //   }
  //   Label _label;

  //   float Value
  //   {
  //     get { return _valueComponent.Value; }
  //     set
  //     {
  //       _valueComponent.Value = value;
  //       var val01 = Utils.Map(value, _min, _max, 0, 1, true);
  //       var empty01 = 1 - val01;
  //       _emptySpace.Style.Set(Styles.Height, empty01 * Height);
  //       _filledSpace.Style.Set(Styles.Height, val01 * Height);
  //     }
  //   }
  //   public VerticalSlider2(string title, Func<float> getValueCallback, Action<float> setValueCallback,
  //     float min = 0, float max = 1, float width = 10f, float height = 50) : base()
  //   {
  //     _min = min;
  //     _max = max;
  //     Width = width;
  //     Height = height;
  //     _valueComponent = new ValueComponent<float>(getValueCallback, setValueCallback);
  //     _verticalLayout = new VerticalLayout(width);
  //     _emptySpace = new EmptySpace(width, 0);
  //     _filledSpace = new EmptySpace(width, 0);
  //     _emptySpace.Style.Set(Styles.BackgroundColor, Col.black);
  //     _filledSpace.Style.Set(Styles.BackgroundColor, Col.white);
  //     _verticalLayout.Attach(_emptySpace, _filledSpace);
  //     _label = new ValueLabel(title, () =>
  //     {
  //       var str = string.Format("{0:0.00}", Value);
  //       float w = Utils.CalcSize(str, Style).x;
  //       if (w > Width * 0.5f)
  //       {
  //         return "";
  //       }
  //       return str;
  //     });
  //     _verticalLayout.Attach(_label);
  //     AddChild(_verticalLayout);
  //     Value = getValueCallback();
  //   }
  //   bool _isDragging;
  //   protected override void OnUpdate()
  //   {
  //     base.OnUpdate();
  //     if (_isDragging)
  //     {
  //       var pos = Utils.MousePosition();
  //       var box = _verticalLayout.Box;
  //       var val = Utils.Map(pos.y - box.y, 0, Height, _max, _min, true);
  //       _valueComponent.Value = val;
  //     }
  //     Value = _valueComponent.Value;
  //   }
  //   protected override void OnDraggingStart(float x, float y)
  //   {
  //     base.OnDraggingStart(x, y);
  //     if (_verticalLayout.Box.Contains(new Vec2(x, y)))
  //     {
  //       _isDragging = true;
  //     }
  //   }
  //   protected override void OnMouseUp()
  //   {
  //     base.OnMouseUp();
  //     _isDragging = false;
  //   }
  // }
}
