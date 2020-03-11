namespace GUIPanels
{
  public class VerticalLayout : DrawableComponent
  {
    public VerticalLayout(float width = 100f) : base()
    {
      Style.Set<float>(Styles.Width, width);
    }

    protected override void Render()
    {
      base.Render();
      // then border
      // first set position of each component
      var pos = ContentPosition;
      float y = pos.y;
      foreach (var comp in Children)
      {
        var box = comp.Box;
        comp.Position = new Vec2(pos.x, y);
        comp.Style.Set(Styles.Width, InnerWidth);
        y += box.height;
        comp.Draw();
      }
    }

    protected override float ContentHeight
    {
      get
      {
        float h = 0;
        foreach (var comp in Children)
        {
          h += comp.Box.height;
        }
        return h;
      }
    }
    protected override float InnerHeight
    {
      get { return ContentHeight; }
    }

  }
}
