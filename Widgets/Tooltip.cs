namespace GUIPanels
{
  public class Tooltip : Paragraph
  {
    public Tooltip(float width) : base(width)
    {
      Style
        .Background(Col.white)
        .FontColor(Col.black)
        .Border(1, Col.black);
    }
  }
}
