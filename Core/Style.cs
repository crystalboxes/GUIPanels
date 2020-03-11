namespace GUIPanels
{
  public abstract class Style
  {
    public abstract float FontSize { get; set; }
    public abstract Col FontColor { get; set; }

    public Col PrimaryColor = Col.white;
    public Col SecondaryColor = Col.black;
  }
}
