namespace GUIPanels
{
  public interface IValueComponent<T>
  {
    T Value { get; set; }
    string Title { get; set; }
  }
  public class ValueComponent<T> : IValueComponent<T>
  {
    public T Value
    {
      get { return _getValueCallback == null ? default(T) : _getValueCallback(); }
      set { if (_setValueCallback != null) _setValueCallback(value); }
    }
    public string Title { get; set; }

    System.Func<T> _getValueCallback;
    System.Action<T> _setValueCallback;

    public ValueComponent(string title, System.Func<T> getValueCallback = null, System.Action<T> setValueCallback = null) : base()
    {
      _getValueCallback = getValueCallback;
      _setValueCallback = setValueCallback;
      Title = title;
    }
  }
}
