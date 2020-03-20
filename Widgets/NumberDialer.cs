using System;
namespace GUIPanels
{
  // TODO
  public class NumberDialer : VerticalLayout
  {
    ValueComponent<float> _valueComponent;
    float Value
    {
      get { return _valueComponent.Value; }
      set { _valueComponent.Value = value; }
    }
    string Title { get; set; }
    Label _dialerLabel;
    public NumberDialer(string title, Func<float> getValueCallback, Action<float> setValueCallback) : base()
    {
      _valueComponent = new ValueComponent<float>(getValueCallback, setValueCallback);
      Title = title;
      _dialerLabel = new Label(title);
      Attach(_dialerLabel);
      // on hover add the cursor
      // +10000.000
    }

    protected override void OnUpdate()
    {
      if (Box.Contains(Utils.MousePosition()))
      {

      }
    }
  }
}