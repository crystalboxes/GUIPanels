namespace GUIPanels
{
  public class Slider : VerticalLayout
  {
    public Slider(string title, System.Func<string> getValueCallback = null, System.Action<string> setValueCallback = null, float width = 100f) : base(width)
    {
    }

    ValueComponent<float> valueComponent;
  }
}