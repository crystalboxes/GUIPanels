using System;

namespace GUIPanels
{
  public interface IParameter
  {
    float Width { get; set; }
    System.Func<bool> HideWhen { get; set; }
    float Height { get; }
    void Repaint();
    void UpdateStyle();


    BasePanel Owner { get; set; }

    Vec2 Position { get; set; }
  }

  public abstract class Parameter : IParameter
  {
    public abstract void UpdateStyle();
    public System.Func<bool> HideWhen = () => false;

    public virtual float Width { get; set; }
    public abstract float Height { get; }

    public Vec2 Position { get; set; }
    public BasePanel Owner { get; set; }

    Func<bool> IParameter.HideWhen
    {
      get
      {
        return this.HideWhen;
      }

      set
      {
        this.HideWhen = value;
      }
    }

    public abstract void Repaint();

  }
}
