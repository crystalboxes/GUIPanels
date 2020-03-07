namespace GUIPanels
{
  public interface IParameter
  {
    float Width { get; set; }
    float Height { get; }
    void Repaint();
    void UpdateStyle();


    BasePanel Owner { get; set; }

    Vec2 Position { get; set; }
  }

  public abstract class Parameter : IParameter
  {
    public abstract void UpdateStyle();

    public virtual float Width { get; set; }
    public abstract float Height { get; }

    public Vec2 Position { get; set; }
    public BasePanel Owner { get; set; }

    public abstract void Repaint();

  }
}
