namespace GUIPanels
{
  public class EmptySpace : DrawableComponent
  {
    public EmptySpace(float width, float height) : base()
    {
      Style.Set(Styles.Width, width);
      Style.Set(Styles.Height, height);
    }
  }
}
