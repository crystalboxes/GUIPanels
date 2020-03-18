namespace GUIPanels
{
  public class EmptySpace : Widget
  {
    public EmptySpace(float width = 10f, float height = 10f) : base()
    {
      Style.Set(Styles.Width, width);
      Style.Set(Styles.Height, height);
    }
  }
}
