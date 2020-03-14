using System.Collections.Generic;

namespace GUIPanels
{
  public interface IDrawableComponent
  {
    Vec2 Position { get; set; }
    Rectangle Box { get; }
    ElementStyle Style { get; }
    List<IDrawableComponent> Children { get; }
    IDrawableComponent Parent { get; }
    void Draw();
    void SetParent(IDrawableComponent parent);
    void AddChild(IDrawableComponent child);
  }
}
