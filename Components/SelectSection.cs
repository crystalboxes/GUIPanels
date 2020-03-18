namespace GUIPanels
{
  public abstract class SelectPanel : VerticalLayout
  {
    public Widget Container;
    public SelectPanel(string[] options, int selectedIndex) : base(100) { }
    bool[] _options;
    public int SelectedIndex
    {
      get
      {
        for (int x = 0; x < _options.Length; x++)
        {
          if (_options[x]) return x;
        }
        return -1;
      }
    }
    protected void Construct(string[] options, int selectedIndex, Widget component)
    {
      _options = new bool[options.Length];
      System.Diagnostics.Debug.Assert(_options.Length > selectedIndex);

      int index = 0;
      foreach (var option in options)
      {
        component.Attach(new RadioToggle(option, () =>
        {
          return _options[System.Array.IndexOf(options, option)];
        }, x =>
        {
          if (x)
          {
            SetActive(System.Array.IndexOf(options, option));
          }
        }));
        index++;
      }
      AddChild(component);
      Container = component;
    }

    protected void SetActive(int index)
    {
      for (int x = 0; x < _options.Length; x++)
      {
        _options[x] = false;
      }
      _options[index] = true;
    }
  }
  public class SelectHorizontal : SelectPanel
  {
    public SelectHorizontal(string[] options, int selectedIndex) : base(options, selectedIndex)
    {
      var verticalLayout = new HorizontalGrid();
      Construct(options, selectedIndex, verticalLayout);
      SetActive(selectedIndex);
    }
  }
  public class SelectVertical : SelectPanel
  {
    public SelectVertical(string[] options, int selectedIndex) : base(options, selectedIndex)
    {
      var verticalLayout = new VerticalLayout(Box.width);
      Construct(options, selectedIndex, verticalLayout);
      SetActive(selectedIndex);
    }
  }
}
