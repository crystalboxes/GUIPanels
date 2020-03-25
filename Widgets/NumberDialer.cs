using System;
namespace GUIPanels
{
  // TODO
  public class NumberDialer : VerticalLayout
  {
    public Widget NumberLabel { get { return _numberLabel; } }
    public Widget TextLabel { get { return _textLabel; } }
    public ElementStyle LabelStyle { get { return _labelStyle; } }
    ValueComponent<float> _valueComponent;
    float Value
    {
      get { return _valueComponent.Value; }
      set { _valueComponent.Value = value; }
    }
    string Title { get; set; }
    Label _textLabel, _numberLabel;
    char[] _valueArr;
    string _displayValue;
    string _formatString;
    void SetDisplayArr()
    {
      var value = Value;
      var sign = value < 0 ? '-' : '+';

      _displayValue = sign + string.Format(_formatString, System.Math.Abs(value));
      _valueArr = _displayValue.ToCharArray();

      for (int x = 0; x < _valueArr.Length; x++)
      {
        _numberLabels[x].Title = "" + _valueArr[x];
      }
    }
    int _integerNums, _precision;
    ElementStyle _labelStyle;

    Label[] _numberLabels;

    Widget _currentDraggingLabel = null,
            _currentHoveredLabel = null;
    float _cachedValue = 0;
    float _min, _max;

    public NumberDialer(string title, Func<float> getValueCallback, Action<float> setValueCallback,
      float min = -10000, float max = 10000, int precision = 3) : base()
    {
      _min = min; _max = max;
      _valueComponent = new ValueComponent<float>(getValueCallback, setValueCallback);
      Title = title;
      // if min < 0 then need  +- switch
      // if max and min < 0 then always - 
      // 1 is for sign
      int numCount = 1; // sign
      var rmin = ((int)System.Math.Abs(min)).ToString().Length;
      var rmax = ((int)System.Math.Abs(max)).ToString().Length;
      _integerNums = System.Math.Max(rmin, rmax);
      numCount += _integerNums;
      numCount++; // .
      _precision = precision;
      numCount += _precision; // fractional
      {
        _formatString = "{0:";
        for (int i = 0; i < _integerNums; i++) { _formatString += '0'; }
        _formatString += '.';
        for (int i = 0; i < _precision; i++) { _formatString += '0'; }
        _formatString += "}";
      }

      _valueArr = new char[numCount];
      _numberLabels = new Label[_valueArr.Length];
      var index = 0;

      foreach (var ch in _valueArr)
      {
        var label = new Label();

        if (index == 0)
        {
          _labelStyle = label.Style;
        }
        else
        {
          label.SetStyle(_labelStyle);
        }
        label
          .AddEventListener(EventType.MouseDown, () => _currentDraggingLabel = label)
          .AddEventListener(EventType.Hover, () => _currentHoveredLabel = label);
        _numberLabels[index++] = label;
      }

      // add horizontal layout
      Attach(new HorizontalLayout()
        .Attach(new Label(" ").SetStyle(_labelStyle))
        .Attach(_numberLabels)
        .Attach(new Label(" ").SetStyle(_labelStyle))
        .Attach(
          _textLabel = new Label(title)
        )
      );
    }

    protected override void Render()
    {
      base.Render();

      if (!Box.Contains(Utils.MousePosition()))
      {
        _currentHoveredLabel = null;
      }

      if (_currentHoveredLabel != null || _currentDraggingLabel != null)
      {
        var l = _currentHoveredLabel;
        if (l == null) { l = _currentDraggingLabel; }
        var rect = new Rectangle(l.Box.x, l.Box.y + l.Box.height * 0.9f, l.Box.width, l.Box.height * 0.1f);
        Rendering.DrawRect(rect, CurrentStyle.FontColor());
      }
    }

    protected override void OnMouseUp()
    {
      base.OnMouseUp();
      _currentDraggingLabel = null;
    }

    protected override void OnUpdate()
    {
      base.OnUpdate();
      // Check if sign
      if (_currentDraggingLabel != null)
      {
        HandleDragging();
      }
      // Update display
      if (!Single.Equals(_cachedValue, Value))
      {
        _cachedValue = Value;
        SetDisplayArr();
      }
    }

    float _previousY, _currentY;
    const float Multiplier = 0.5f;
    void HandleDragging()
    {
      Func<float, float> Clamp = x => Utils.Clamp(x, _min, _max);
      // get y relative to mouse pos
      var pos = Utils.MousePosition();
      var box = Box;
      //
      float y = Utils.MousePosition().y - Box.y;
      _currentY = y;
      float delta = (y - _previousY) * -1f;
      _previousY = _currentY;

      var label = _currentDraggingLabel;
      var index = Array.IndexOf(_numberLabels, label);

      if (index == 0)
      {
        Value = Clamp((float)System.Math.Abs(Value) * (_currentY < 0 ? 1 : -1));
        return;
      }

      int indexOfDot = _integerNums + 1;
      if (index == indexOfDot)
      {
        return;
      }

      var exp = indexOfDot - index;
      if (exp > 0)
      {
        exp--;
      }

      float incr = (float)System.Math.Pow(10, exp);

      Value = Clamp(Value + incr * delta * Multiplier);
    }
  }
}
