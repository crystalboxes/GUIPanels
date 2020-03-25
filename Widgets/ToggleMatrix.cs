namespace GUIPanels
{
  public class ToggleMatrix : VerticalLayout
  {
    class BoolInstance
    {
      public bool Value;
    }
    BoolInstance[][] toggleGrid;
    public ElementStyle ToggleStyle { get { return _toggleStyle; } }
    ElementStyle _toggleStyle;
    public ToggleBase[] Toggles;

    public ToggleMatrix(string text, int x = 2, int y = 2)
    {
      var z = x;
      x = y;
      y = z;
      toggleGrid = new BoolInstance[y][];
      var index = 0;
      Toggles = new ToggleBase[x * y];
      for (int i = 0; i < y; i++)
      {
        toggleGrid[i] = new BoolInstance[x];
        var grid = new HorizontalLayout();
        for (int j = 0; j < x; j++)
        {
          var instance = new BoolInstance();
          toggleGrid[i][j] = instance;
          var toggle = new Toggle("", () => instance.Value, a => instance.Value = a)
            .SetLabelHidden();
          Toggles[index] = toggle;
          if (index++ == 0)
          {
            _toggleStyle = toggle.Style;
          }
          toggle.SetStyle(_toggleStyle);
          grid.Attach(toggle);
        }
        Attach(grid);
      }
    }
  }
}