namespace GUIPanels
{
  public class WindowPanel : VerticalLayout
  {
    public VerticalLayout Container { get { return _container; } }
    public HeaderWidget Header { get { return _header; } }
    public float HeaderOffset
    {
      get { return Header.CurrentStyle.Get<Dim>(Styles.Margin).Bottom; }
      set { Header.CurrentStyle.Set<Dim>(Styles.Margin, Dim.bottom * value); }
    }
    HeaderWidget _header;
    VerticalLayout _container;
    public WindowPanel(string title, float width = 165) : base(width)
    {
      _header = new HeaderWidget(title,
        x => _container.Style.Set(Styles.Hidden, !x));
      _container = new VerticalLayout(width);
      base.AddChild(_header);
      base.AddChild(_container);
    }
    protected override void AddChild(Widget child)
    {
      _container.Attach(child);
    }

    protected override void ApplyChildStyles()
    {
      if (SetChildStyle != null)
      {
        foreach (var child in _container.Children)
        {
          SetChildStyle(child);
        }
      }
    }

    // handle dragging
    Vec2 _dragOffset;
    bool _isDragging = false;
    protected override void OnDraggingStart(float x, float y)
    {
      base.OnDraggingStart(x, y);
      var pos = new Vec2(x, y);
      if (_header.Box.Contains(pos))
      {
        _isDragging = true;
        _dragOffset = Position - pos;
      }
    }
    protected override void OnMouseUp()
    {
      base.OnMouseUp();
      _isDragging = false;
    }
    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (!_isDragging)
      {
        return;
      }
      Position = Utils.MousePosition() + _dragOffset;
    }
  }
}
