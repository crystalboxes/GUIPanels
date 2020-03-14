namespace GUIPanels
{
  public class WindowPanel : VerticalLayout
  {
    public VerticalLayout Container { get { return _container; } }
    public HeaderComponent Header { get { return _header; } }
    public float HeaderOffset
    {
      get { return Header.Style.Get<Dim>(Styles.Margin).Bottom; }
      set { Header.Style.Set<Dim>(Styles.Margin, Dim.bottom * value); }
    }
    HeaderComponent _header;
    VerticalLayout _container;
    public WindowPanel(string title, float width = 165) : base(width)
    {
      _header = new HeaderComponent(title, x =>
      {
        _container.Style.Set(Styles.Hidden, !x);
      });
      HeaderOffset = 4;
      _container = new VerticalLayout(width);
      _container.Style.Set(Styles.BackgroundColor, new Col(1, 1, 1, .55f));

      base.AddChild(_header);
      base.AddChild(_container);
    }
    public override void AddChild(IDrawableComponent child)
    {
      _container.AddChild(child);
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
