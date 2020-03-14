namespace GUIPanels
{
  public interface IValueComponent<T>
  {
    T Value { get; set; }
  }
  public class ValueComponent<T> : IValueComponent<T>
  {
    public T Value
    {
      get { return _getValueCallback == null ? default(T) : _getValueCallback(); }
      set { if (_setValueCallback != null) _setValueCallback(value); }
    }
    System.Func<T> _getValueCallback;
    System.Action<T> _setValueCallback;
    public ValueComponent( System.Func<T> getValueCallback = null, System.Action<T> setValueCallback = null) : base()
    {
      _getValueCallback = getValueCallback;
      _setValueCallback = setValueCallback;
    }
  }
}
