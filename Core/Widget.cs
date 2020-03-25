using System.Collections.Generic;

namespace GUIPanels
{
  public enum EventType
  {
    Click, DoubleClick, MouseUp, MouseDown, Hover
  }
  public abstract class Widget
  {
    public enum State { Idle, Clicked, Hovered }

    State EventState { get { return _state; } }
    State _state = State.Idle;

    public Widget()
    {
      _style = new ElementStyle(this);
      Style.Set(Styles.BackgroundColor, new Col(0, 0, 0, 0));
      // TODO set default styles
      Style.Set(Styles.Padding, Dim.Zero);
      Style.Set(Styles.Margin, Dim.Zero);
      Style.Set(Styles.Border, Dim.Zero);
    }

    public static T New<T>(params object[] list)
    {
      return (T)System.Activator.CreateInstance(typeof(T), list);
    }

    public Widget Attach(params Widget[] children)
    {
      foreach (var child in children)
      {
        AddChild(child);
      }
      return this;
    }
    public Widget Attach(Widget child)
    {
      AddChild(child);
      return this;
    }
    Tooltip _tooltip;
    public Widget SetTooltip(string text)
    {
      if (_tooltip == null)
      {
        _tooltip = new Tooltip(100);
      }
      _tooltip.SetText(text);
      _tooltip.CalculateTextBox();
      return this;
    }

    public T Cast<T>() where T : Widget
    {
      return (T)this;
    }

    public Widget ApplyChildStyle(System.Action<Widget> StyleAction)
    {
      SetChildStyle = StyleAction;
      return this;
    }

    public Widget SetPositionCallbacks(System.Func<Vec2> getPositionCallback,
      System.Action<Vec2> setPositionCallback)
    {
      _getPositionCallback = getPositionCallback;
      _setPositionCallback = setPositionCallback;
      return this;
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
        RecalculateBox();
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
      get { return CurrentStyle.Get<float>(Styles.Width); }
    }

    protected virtual float ContentHeight
    {
      get { return CurrentStyle.Get<float>(Styles.Height); }
    }

    protected virtual float InnerWidth
    {
      get { return ContentWidth - _border.Left - _border.Right - _padding.Left - _padding.Right; }
    }

    protected virtual float InnerHeight
    {
      get { return ContentHeight - _border.Top - _border.Bottom - _padding.Top - _padding.Bottom; }
    }

    Dim Margin { get { return _margin; } }

    Dim Padding { get { return _padding; } }

    Dim Border { get { return _border; } }

    Dim _margin, _padding, _border;
    Vec2 _position;
    Rectangle _contentBox, _box;
    float _innerWidth, _innerHeight;

    public void RecalculateBox()
    {
      _position = CurrentStyle.Position();
      _margin = CurrentStyle.Get<Dim>(Styles.Margin);
      _padding = CurrentStyle.Get<Dim>(Styles.Padding);
      _border = CurrentStyle.Get<Dim>(Styles.Border);

      _innerWidth = InnerWidth;
      _innerHeight = InnerHeight;

      _contentBox = new Rectangle(ContentPosition.x, ContentPosition.y, _innerWidth, _innerHeight);

      if (!CurrentStyle.Get<bool>(Styles.Hidden))
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

    protected virtual void AddChild(Widget child)
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

    public ElementStyle Style { get { return _style; } }
    public Widget SetStyle(ElementStyle style)
    {
      _style = style;
      return this;
    }
    public virtual ElementStyle CurrentStyle
    {
      get
      {
        if (_state == State.Clicked)
        {
          return Style.Clicked as ElementStyle;
        }
        if (_state == State.Hovered)
        {
          return Style.Hovered as ElementStyle;
        }
        return Style;
      }
    }
    public Widget Parent { get { return _parent; } }
    public List<Widget> Children { get { return _children; } }

    bool _isThemeApplied;

    void Initialize()
    {
      Theme.Current.Apply(this);
      ApplyChildStyles();
      _isThemeApplied = true;
      RecalculateBox();
    }

    public void Draw()
    {
      if (!_isThemeApplied)
      {
        Initialize();
      }

      if (CurrentStyle.Get<bool>(Styles.Hidden))
      {
        return;
      }

      RecalculateBox();

      // update all events
      UpdateEvents();
      var marginOffset = new Vec2(Position.x + _margin.Left, Position.y + _margin.Top);
      // Raw background
      var contentBox = ContentBox;
      contentBox.height += _padding.Top + _padding.Bottom;
      contentBox.width += _padding.Left + _padding.Right;
      contentBox.x -= _padding.Left;
      contentBox.y -= _padding.Top;
      var bg = CurrentStyle.Get<Col>(Styles.BackgroundColor);
      if (bg.a > 0.001f)
      {
        Rendering.DrawRect(contentBox, bg);
      }

      // Draw border
      var borderColor = CurrentStyle.Get<Col>(Styles.BorderColor);
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

      if (CurrentStyle.Exists(Styles.Outline))
      {
        var outline = CurrentStyle.Get<GUIPanels.Outline>(Styles.Outline);
        Rendering.DrawLineBox(contentBox, outline.Width, outline.Color);
      }

      if (_shouldShowTooltip)
      {
        if (_tooltip != null)
        {
          _tooltip.Draw();
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
    const float doubleClickInterval = 0.35f;
    bool _mouseEntered = false,
      _shouldShowTooltip = false;


    Dictionary<EventType, List<System.Action>> _eventListeners = new Dictionary<EventType, List<System.Action>>();
    public Widget AddEventListener(EventType eventType, System.Action action)
    {
      if (!_eventListeners.ContainsKey(eventType))
      {
        _eventListeners.Add(eventType, new List<System.Action>());
      }
      _eventListeners[eventType].Add(action);
      return this;
    }
    void DispatchEvents(EventType eventType)
    {
      if (!_eventListeners.ContainsKey(eventType))
      {
        return;
      }
      foreach (var e in _eventListeners[eventType])
      {
        e();
      }
    }
    float lastClickTime;
    void UpdateEvents()
    {
      OnUpdate();
      _shouldShowTooltip = false;

      var mousePos = Utils.MousePosition();
      if (Box.Contains(mousePos))
      {
        DispatchEvents(EventType.Hover);

        _shouldShowTooltip = true;
        if (_tooltip != null)
        {
          _tooltip.Position = mousePos + new Vec2(11, 11);
        }
        if (!_mouseEntered)
        {
          OnMouseEntered(mousePos.x, mousePos.y);
          _mouseEntered = true;
        }
        if (_state != State.Clicked)
        {
          _state = State.Hovered;
        }

        if (Utils.GetMouseDown())
        {
          _state = State.Clicked;
          DispatchEvents(EventType.MouseDown);
          OnDraggingStart(mousePos.x, mousePos.y);
          _mouseDownTime = Utils.Time;
        }
      }
      else
      {
        if (_state != State.Clicked)
        {
          _state = State.Idle;
        }
        _mouseEntered = false;
      }

      _mouseUpPrev = _mouseUp;
      _mouseUp = Utils.GetMouseUp();

      if (MouseUp)
      {
        _state = State.Idle;
        DispatchEvents(EventType.MouseUp);
        OnMouseUp();
        if ((Utils.Time - _mouseDownTime) < clickInterval)
        {
          DispatchEvents(EventType.Click);
          OnClick();
          _clicks++;
          if (_clicks == 2)
          {
            _clicks = 0;
            DispatchEvents(EventType.DoubleClick);
            OnDoubleClick();
          }
          lastClickTime = Utils.Time;
        }
      }
      if (Utils.Time - lastClickTime > doubleClickInterval)
      {
        _clicks = 0;
      }
    }
    int _clicks = 0;

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

    protected virtual void OnDoubleClick()
    {
    }
  }
}