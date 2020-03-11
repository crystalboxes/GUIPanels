namespace GUIPanels
{
  public class VerticalLayout : DrawableComponent
  {
    public VerticalLayout(float width) : base()
    {
      Style.Set<float>(Styles.Width, width);
    }

    public override void AddChild(IDrawableComponent child)
    {
      base.AddChild(child);
      // Remove child property so it inherits from the parent
      child.Style.RemoveProperty(Styles.Width);
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
