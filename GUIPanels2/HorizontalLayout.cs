namespace GUIPanels
{
  public class HorizontalLayout : DrawableComponent
  {
    bool _useRightAlignment;
    public HorizontalLayout(bool useRightAlignment = false) : base()
    {
      _useRightAlignment = useRightAlignment;
    }

    protected override void Render()
    {
      var pos = ContentPosition;
      System.Action<IDrawableComponent> draw = comp =>
      {
        var box = comp.Box;
        comp.Position = pos;
        comp.Draw();
        pos.x += box.width;
      };
      if (_useRightAlignment)
      {
        // find total width
        float w = 0;
        foreach (var ch in Children)
        {
          w += ch.Box.width;
        }
        pos.x += InnerWidth - w;

        for (int i = Children.Count - 1; i >= 0; i--)
        {
          draw(Children[i]);
        }
      }
      else
      {
        for (int i = 0; i < Children.Count; i++)
        {
          draw(Children[i]);
        }
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
  }
}