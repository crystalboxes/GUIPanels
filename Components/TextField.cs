namespace GUIPanels
{
  public class TextField : VerticalLayout
  {
    ValueComponent<string> _valueComponent;
    public HorizontalGrid TextFieldBox { get { return _horizontalGrid; } }
    HorizontalGrid _horizontalGrid;
    Label _textFieldLabel;
    public string Value
    {
      get { return _valueComponent.Value; }
      set
      {
        _valueComponent.Value = value;
        _textFieldLabel.Title = _valueComponent.Value;
      }
    }
    public TextField(string title, System.Func<string> getValueCallback = null, System.Action<string> setValueCallback = null)
    {
      _valueComponent = new ValueComponent<string>(getValueCallback, setValueCallback);
      AddChild(new Label(title));
      _horizontalGrid = new HorizontalGrid();
      _textFieldLabel = new Label(getValueCallback != null ? getValueCallback() : "");
      _horizontalGrid.AddChild(_textFieldLabel);
      _horizontalGrid.Style.Set(Styles.BackgroundColor, Col.black);
      _textFieldLabel.Style.Set(Styles.FontColor, Col.white);
      AddChild(_horizontalGrid);
    }
    bool _textInputActive;
    protected override void OnMouseUp()
    {
      base.OnMouseUp();
      var pos = Utils.MousePosition();
      if (_horizontalGrid.Box.Contains(pos) && !_textInputActive)
      {
        _textInputActive = true;
      }
      else
      {
        _textInputActive = false;
      }
    }
    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (_textInputActive)
      {
        foreach (char c in Utils.GetInputString())
        {
          if (c == '\b')
          {
            if (Value.Length != 0)
            {
              Value = Value.Substring(0, Value.Length - 1);
            }
          }
          else if ((c == '\n') || (c == '\r'))
          {
            _textInputActive = !_textInputActive;
          }
          else
          {
            Value += c;
          }
        }
      }
    }
    const float _timeInterval = 1;
    protected override void Render()
    {
      base.Render();
      if (!_textInputActive) {
        return;
      }
      // draw the cursor
      if ((Utils.Time % _timeInterval) > (_timeInterval / 2f)) {
        var size = Utils.CalcSize(Value, _textFieldLabel.Style);
        var box = _horizontalGrid.Box;
        float xpos = box.x + size.x;
        Rendering.DrawRect(new Rectangle(xpos, box.y, 1, box.height), 
          _textFieldLabel.Style.Get<Col>(Styles.FontColor));
      }
    }
  }
}