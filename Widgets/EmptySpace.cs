namespace GUIPanels
{
  public class EmptySpace : Widget
  {
    public float Width
    {
      get { return CurrentStyle.Get<float>(Styles.Width); }
      set { CurrentStyle.Set<float>(Styles.Width, value); }
    }
    public float Height
    {
      get { return CurrentStyle.Get<float>(Styles.Height); }
      set { CurrentStyle.Set<float>(Styles.Height, value); }
    }
    public EmptySpace(float width = 10f, float height = 10f) : base()
    {
      Style.Set(Styles.Width, width);
      Style.Set(Styles.Height, height);
    }
  }

  public class Spacer : HorizontalGrid
  {
    public float Height
    {
      get { return _space.Style.Get<float>(Styles.Height); }
      set { _space.Style.Set<float>(Styles.Height, value); }
    }
    Widget _space;
    public Spacer(float height = 10f) : base()
    {
      Attach(_space = new EmptySpace(1, height));
    }
  }
}
