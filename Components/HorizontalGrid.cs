namespace GUIPanels
{
  public class HorizontalGrid : DrawableComponent
  {
    float ChildWidth { get { return InnerWidth / (float)Children.Count; } }

    protected override void Render()
    {
      float childWidth = ChildWidth;
      var pos = ContentPosition;
      foreach (var child in Children)
      {
        child.Style.Set(Styles.Width, childWidth);
        child.Position = pos;
        pos.x += childWidth;
        child.Draw();
      }
    }
    protected override float ContentHeight
    {
      get
      {
        float max = 0;
        foreach (var comp in Children)
        {
          float h = comp.Box.height;
          if (h > max)
          {
            max = h;
          }
        }
        return max;
      }
    }

    protected override float InnerHeight { get { return ContentHeight; } }
  }
}