namespace GUIPanels
{
  public class HorizontalGrid : LayoutBase
  {
    float ChildWidth { get { return InnerWidth / (float)Children.Count; } }

    protected override void Render()
    {
      float childWidth = ChildWidth;
      var pos = ContentPosition;
      bool alignToCenter = Children.Count == 1 && _alignToCenter;

      System.Action<Widget> setAlignedToLeft = child =>
      {
        child.Style.Set(Styles.Width, childWidth);
        child.Position = pos;
        pos.x += childWidth;
      };

      foreach (var child in Children)
      {
        if (alignToCenter)
        {
          if (!_initialized)
          {
            _initialized = true;
            child.RecalculateBox();
          }
          float w = child.Box.width;
          if (w > InnerWidth)
          {
            setAlignedToLeft(child);
          }
          else
          {
            child.Position = new Vec2(InnerWidth / 2f - w / 2f + pos.x, pos.y);
          }
        }
        else
        {
          setAlignedToLeft(child);
        }
        child.Draw();
      }
    }
    bool _alignToCenter = false;
    bool _initialized;
    public HorizontalGrid SetAlignToCenter(bool value = true)
    {
      _alignToCenter = value;
      return this;
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
