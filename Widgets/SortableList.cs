namespace GUIPanels
{
  public class SortableList : VerticalLayout
  {
    public ElementStyle ItemStyle
    {
      get { return _itemStyle; }
    }
    ElementStyle _itemStyle;
    public SortableList(string[] items)
    {
      var index = 0;
      foreach (var item in items)
      {
        var grid = new HorizontalGrid();
        if (index++ == 0) { _itemStyle = grid.Style; }
        grid.SetStyle(_itemStyle);
        Attach(
          ((HorizontalGrid)grid.Attach(new Label(item))).SetAlignToCenter()
        );
      }
    }
  }
}