using System;

namespace GUIPanels
{
  public class ButtonToggle : Button
  {
    public override ElementStyle CurrentStyle
    {
      get { return Value ? base.Style.Clicked : base.CurrentStyle; }
    }

    ValueComponent<bool> _valueComponent;
    public bool Value
    {
      get { return _valueComponent != null ? _valueComponent.Value : default(bool); }
      set { _valueComponent.Value = value; }
    }
    public ButtonToggle(string title,
      Func<bool> getValueCallback, Action<bool> setValueCallback) : base(title, null)
    {
      _valueComponent = new ValueComponent<bool>(getValueCallback, setValueCallback);
      SetOnClickCallback(() =>
      {
        Value = !Value;
      });

      Theme.Current.Apply(this, typeof(Button));
    }
  }

  public class Button : VerticalLayout
  {
    public Label Label { get { return _label; } }

    Col _primaryColor = Col.black,
      _secondaryColor = Col.white;

    Label _label;
    System.Action _onClickCallback;
    bool _clicked;

    public Button(string title, System.Action onClickCallback = null) : base()
    {
      _onClickCallback = onClickCallback;
      _label = new Label(title);
      _label.SetParent(this);
      var size = Utils.CalcSize(_label.Title, _label.Style);
      AddChild(new EmptySpace(0, size.y));
    }

    public Button SetOnClickCallback(Action onClickCallback)
    {
      _onClickCallback = onClickCallback;
      return this;
    }

    protected override void OnDraggingStart(float x, float y)
    {
      base.OnDraggingStart(x, y);
      _clicked = true;
    }
    protected override void OnMouseUp()
    {
      base.OnMouseUp();
      if (_clicked && _onClickCallback != null)
      {
        _onClickCallback();
      }
      _clicked = false;
    }

    protected override void Render()
    {
      base.Render();
      // Draw text horizontally aligned.
      var pos = ContentPosition;
      var size = Utils.CalcSize(Label.Title, _label.Style);
      var center = new Vec2(pos.x + InnerWidth * 0.5f,
        pos.y + InnerHeight * 0.5f);
      var textP = new Vec2(center.x - size.x * 0.5f, center.y - size.y * 0.5f);
      Label.Position = textP;
      Label.Draw();
    }
  }
}
