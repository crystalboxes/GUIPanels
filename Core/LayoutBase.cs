namespace GUIPanels
{
  public class LayoutBase : Widget
  {
    public LayoutBase() : base()
    {
      Theme.Current.Apply(this, typeof(LayoutBase));

    }
  }
}
