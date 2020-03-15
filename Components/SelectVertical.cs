namespace GUIPanels
{
  public class SelectVertical : VerticalLayout
  {
    bool[] _options;
    public int SelectedIndex { get {for (int x = 0; x < _options.Length; x++) {} }}
    public SelectVertical(string[] options, int selectedIndex)
    {
      _options = new bool[options.Length];
      System.Diagnostics.Debug.Assert(_options.Length > selectedIndex);

      int index = 0;
      foreach (var option in options)
      {
        AddChild(new RadioToggle(option, () => _options[index], x => { if (x) { SetActive(index); } }));
        index++;
      }
    }
    void SetActive(int index)
    {
      for (int x = 0; x < _options.Length; x++)
      {
        _options[x] = false;
      }
      _options[index] = true;
    }
  }
}