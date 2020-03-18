namespace GUIPanels
{
  public class TextureWidget : EmptySpace
  {
    Texture _tex;
    public TextureWidget(Texture texture, float width, float height) : base(width, height)
    {
      _tex = texture;
    }
    protected override void Render()
    {
      Vec2 pos = ContentPosition;
      Rendering.DrawTexture(new Rectangle(pos.x, pos.y, InnerWidth, InnerHeight), _tex);
    }
  }
}
