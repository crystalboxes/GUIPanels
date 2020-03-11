namespace GUIPanels
{
  public class EmptySpace : DrawableComponent
  {
    public EmptySpace(float width = 10f, float height = 10f) : base()
    {
      Style.Set(Styles.Width, width);
      Style.Set(Styles.Height, height);
    }
  }
}
