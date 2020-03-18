using System.Collections.Generic;

namespace GUIPanels
{
  public abstract class Widget
  {
    public Widget()
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

    public static T New<T>(params object[] list)
    {
      return (T)System.Activator.CreateInstance(typeof(T), list);
    }

    public Widget Attach(Widget child)
    {
      AddChild(child);
      return this;
    }

    public void SetPositionCallbacks(System.Func<Vec2> getPositionCallback,
      System.Action<Vec2> setPositionCallback)
    {
      _getPositionCallback = getPositionCallback;
      _setPositionCallback = setPositionCallback;
    }

    System.Func<Vec2> _getPositionCallback;
    System.Action<Vec2> _setPositionCallback;

    public Vec2 Position
    {
      get { return _getPositionCallback != null ? _getPositionCallback() : _position; }
      set
      {
        if (_setPositionCallback != null)
        {
          _setPositionCallback(value);
        }
        else
        {
          _position = value;
        }
        Style.Set<Vec2>(Styles.Position, Position);
        Recalculate();
      }
    }

    public Rectangle ContentBox { get { return _contentBox; } }
    public Rectangle Box { get { return _box; } }

    protected virtual Vec2 ContentPosition
    {
      get
      {
        return new Vec2(
          _margin.Left + _border.Left + _padding.Left + Position.x,
          _margin.Top + _border.Top + _padding.Top + Position.y
        );
      }
    }

    public System.Action<Widget> SetChildStyle { get; set; }

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
      get { return ContentWidth - _border.Left - _border.Right - _padding.Left - _padding.Right; }
    }

    protected virtual float InnerHeight
    {
      get { return ContentHeight - _border.Top - _border.Bottom - _padding.Top - _padding.Bottom; }
    }

    Dim Margin
    {
      get { return _margin; }
    }

    Dim Padding { get { return _padding; } }

    Dim Border { get { return _border; } }

    Dim _margin, _padding, _border;
    Vec2 _position;
    Rectangle _contentBox, _box;
    float _innerWidth, _innerHeight;

    void Recalculate()
    {
      _margin = Style.Get<Dim>(Styles.Margin);
      _padding = Style.Get<Dim>(Styles.Padding);
      _border = Style.Get<Dim>(Styles.Border);

      _innerWidth = InnerWidth;
      _innerHeight = InnerHeight;

      _contentBox = new Rectangle(ContentPosition.x, ContentPosition.y, _innerWidth, _innerHeight);

      if (!Style.Get<bool>(Styles.Hidden))
      {
        _box = new Rectangle();
        _box.x = Position.x;
        _box.y = Position.y;
        // margin left right border padding width
        _box.width = _margin.Left + _border.Left + _padding.Left
                     + _margin.Right + _border.Right + _padding.Right + _innerWidth;
        _box.height = _margin.Top + _border.Top + _padding.Top
                      + _margin.Bottom + _border.Bottom + _padding.Bottom + _innerHeight;
      }
    }

    public virtual void SetParent(Widget parent)
    {
      _parent = parent;
      _getPositionCallback = null;
      _setPositionCallback = null;
    }

    public virtual void AddChild(Widget child)
    {
      Children.Add(child);
      child.SetParent(this);
    }

    protected virtual void ApplyChildStyles()
    {
      if (SetChildStyle != null)
      {
        foreach (var child in Children)
        {
          SetChildStyle(child);
        }
      }
    }

    Widget _parent;
    List<Widget> _children = new List<Widget>();

    ElementStyle _style;

    public ElementStyle Style
    {
      get { return _style; }
    }

    public Widget Parent
    {
      get { return _parent; }
    }

    public List<Widget> Children
    {
      get { return _children; }
    }

    bool _isThemeApplied;

    void Initialize()
    {
      Theme.Current.Apply(this);
      ApplyChildStyles();
      _isThemeApplied = true;
      Recalculate();
    }

    public void Draw()
    {
      if (!_isThemeApplied)
      {
        Initialize();
      }

      if (Style.Get<bool>(Styles.Hidden))
      {
        return;
      }

      Recalculate();

      // update all events
      UpdateEvents();
      var marginOffset = new Vec2(Position.x + _margin.Left, Position.y + _margin.Top);
      // Raw background
      var contentBox = ContentBox;
      contentBox.height += _padding.Top + _padding.Bottom;
      contentBox.width += _padding.Left + _padding.Right;
      contentBox.x -= _padding.Left;
      contentBox.y -= _padding.Top;
      Rendering.DrawRect(contentBox, Style.Get<Col>(Styles.BackgroundColor));

      // Draw border
      var borderColor = Style.Get<Col>(Styles.BorderColor);
      float widthWithBorder = _border.Left + InnerWidth + _padding.Left + _border.Right + _padding.Right;
      float heightWithBorder = _border.Top + InnerHeight + _padding.Top + _border.Bottom + _padding.Bottom;

      if (_border.Top > float.Epsilon)
      {
        Rendering.DrawRect(new Rectangle(marginOffset.x + _border.Left, marginOffset.y,
          widthWithBorder - _border.Right - _border.Left, _border.Top), borderColor);
      }

      if (_border.Left > float.Epsilon)
      {
        Rendering.DrawRect(new Rectangle(marginOffset.x, marginOffset.y,
          _border.Left, heightWithBorder), borderColor);
      }

      if (_border.Right > float.Epsilon)
      {
        Rendering.DrawRect(new Rectangle(marginOffset.x + _border.Left + _padding.Left + InnerWidth + _padding.Right,
          marginOffset.y,
          _border.Right, heightWithBorder), borderColor);
      }

      if (_border.Bottom > float.Epsilon)
      {
        Rendering.DrawRect(new Rectangle(marginOffset.x + _border.Left,
          marginOffset.y + _border.Top + _padding.Top + InnerHeight + _padding.Bottom,
          widthWithBorder - _border.Right - _border.Left, _border.Bottom), borderColor);
      }

      Render();
      foreach (var child in Children)
      {
        var drawable = (child as Widget);
        if (drawable != null)
        {
          drawable.DeferRender();
        }
      }
    }

    public virtual void DeferRender()
    {
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
          if (!Utils.Event.OnClick.Used)
          {
            OnClick();
          }
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