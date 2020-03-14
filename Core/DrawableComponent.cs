
using System.Collections.Generic;

namespace GUIPanels
{
  public abstract class DrawableComponent : IDrawableComponent
  {
    public DrawableComponent()
    {
      _style = new ElementStyle(this);
      Style.Set(Styles.BackgroundColor, new Col(0, 0, 0, 0));
      // TODO set default styles
      Style.Set(Styles.Padding, Dim.Zero);
      Style.Set(Styles.Margin, Dim.Zero);
      Style.Set(Styles.Border, Dim.Zero);
      Style.Set(Styles.FontSize, 10f);
      Style.Set(Styles.FontColor, Col.black);
    }

    public Vec2 Position
    {
      get { return Style.Get<Vec2>(Styles.Position); }
      set { Style.Set<Vec2>(Styles.Position, value); }
    }

    public virtual void SetParent(IDrawableComponent parent)
    {
      _parent = parent;
    }

    public virtual void AddChild(IDrawableComponent child)
    {
      Children.Add(child);
      child.SetParent(this);
    }

    public Rectangle ContentBox
    {
      get
      {
        var pos = ContentPosition;
        return new Rectangle(pos.x, pos.y, InnerWidth, InnerHeight);
      }
    }

    protected virtual float ContentWidth
    {
      get { return Style.Get<float>(Styles.Width); }
    }

    protected virtual float ContentHeight
    {
      get { return Style.Get<float>(Styles.Height); }
    }

    protected virtual float InnerWidth
    {
      get { return ContentWidth - Border.Left - Border.Right - Padding.Left - Padding.Right; }
    }

    protected virtual float InnerHeight
    {
      get { return ContentHeight - Border.Top - Border.Bottom - Padding.Top - Padding.Bottom; }
    }

    protected virtual Vec2 ContentPosition
    {
      get
      {
        Dim margin = Margin, padding = Padding, border = Border;
        return new Vec2(
          margin.Left + border.Left + padding.Left + Position.x,
          margin.Top + border.Top + padding.Top + Position.y
        );
      }
    }

    Dim Margin
    {
      get { return Style.Get<Dim>(Styles.Margin); }
    }

    Dim Padding
    {
      get { return Style.Get<Dim>(Styles.Padding); }
    }

    Dim Border
    {
      get { return Style.Get<Dim>(Styles.Border); }
    }

    IDrawableComponent _parent;
    List<IDrawableComponent> _children = new List<IDrawableComponent>();

    public Rectangle Box
    {
      get
      {
        var rect = new Rectangle();
        if (!Style.Get<bool>(Styles.Hidden))
        {
          rect.x = Position.x;
          rect.y = Position.y;
          // margin left right border padding width
          Dim margin = Margin, padding = Padding, border = Border;
          rect.width = margin.Left + border.Left + padding.Left
                       + margin.Right + border.Right + padding.Right + InnerWidth;
          rect.height = margin.Top + border.Top + padding.Top
                        + margin.Bottom + border.Bottom + padding.Bottom + InnerHeight;
        }

        return rect;
      }
    }

    ElementStyle _style;

    public ElementStyle Style
    {
      get { return _style; }
    }

    public IDrawableComponent Parent
    {
      get { return _parent; }
    }

    public List<IDrawableComponent> Children
    {
      get { return _children; }
    }

    public void Draw()
    {
      if (Style.Get<bool>(Styles.Hidden))
      {
        return;
      }


      // update all events
      UpdateEvents();
      Dim margin = Margin, padding = Padding, border = Border;
      var marginOffset = new Vec2(Position.x + margin.Left, Position.y + margin.Top);
      // Raw background
      var contentBox = ContentBox;
      contentBox.height += padding.Top + padding.Bottom;
      contentBox.width += padding.Left + padding.Right;
      contentBox.x -= padding.Left;
      contentBox.y -= padding.Top;
      Rendering.DrawRect(contentBox, Style.Get<Col>(Styles.BackgroundColor));

      // Draw border
      var borderColor = Style.Get<Col>(Styles.BorderColor);
      float widthWithBorder = border.Left + InnerWidth + padding.Left + border.Right + padding.Right;
      float heightWithBorder = border.Top + InnerHeight + padding.Top + border.Bottom + padding.Bottom;

      if (border.Top > float.Epsilon)
      {
        Rendering.DrawRect(new Rectangle(marginOffset.x, marginOffset.y,
          widthWithBorder, border.Top), borderColor);
      }

      if (border.Left > float.Epsilon)
      {
        Rendering.DrawRect(new Rectangle(marginOffset.x, marginOffset.y,
          border.Left, heightWithBorder), borderColor);
      }

      if (border.Right > float.Epsilon)
      {
        Rendering.DrawRect(new Rectangle(marginOffset.x + border.Left + padding.Left + InnerWidth + padding.Right,
          marginOffset.y,
          border.Right, heightWithBorder), borderColor);
      }

      if (border.Bottom > float.Epsilon)
      {
        Rendering.DrawRect(new Rectangle(marginOffset.x,
          marginOffset.y + border.Top + padding.Top + InnerHeight + padding.Bottom,
          widthWithBorder, border.Bottom), borderColor);
      }

      Render();
    }

    protected virtual void Render()
    {
    }

    float _mouseDownTime = 0;
    const float clickInterval = 0.3f;
    bool _mouseEntered = false;
    void UpdateEvents()
    {
      OnUpdate();

      var mousePos = Utils.MousePosition();
      if (Box.Contains(mousePos))
      {
        if (!_mouseEntered)
        {
          OnMouseEntered(mousePos.x, mousePos.y);
          _mouseEntered = true;
        }

        if (Utils.GetMouseDown())
        {
          OnDraggingStart(mousePos.x, mousePos.y);
          _mouseDownTime = Utils.Time;
        }
      }
      else
      {
        _mouseEntered = false;
      }


      _mouseUpPrev = _mouseUp;
      _mouseUp = Utils.GetMouseUp();

      if (MouseUp)
      {
        OnMouseUp();
        if ((Utils.Time - _mouseDownTime) < clickInterval)
        {
          OnClick();
        }
      }
    }


    bool _mouseUp;
    bool _mouseUpPrev;

    public bool MouseUp
    {
      get { return _mouseUp && !_mouseUpPrev; }
    }

    protected virtual void OnMouseEntered(float x, float y)
    {
    }
    protected virtual void OnDraggingStart(float x, float y)
    {
    }
    protected virtual void OnMouseUp()
    {
    }
    protected virtual void OnUpdate()
    {
    }
    protected virtual void OnClick()
    {
    }
  }
}
