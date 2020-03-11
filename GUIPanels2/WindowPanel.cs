namespace GUIPanels
{
  public class WindowPanel : VerticalLayout
  {
    public class HeaderComponent : VerticalLayout
    {
      string _title;
      HorizontalGrid _layout;
      public HeaderComponent(string title, System.Action<bool> onToggleClicked) : base(100)
      {
        _title = title;
        Style.Set(Styles.BackgroundColor, new Col(0.5f, 0.5f, 0.5f, 1));
        Style.Set(Styles.Height, 12f);
        Style.Set(Styles.Padding, Dim.bottom * 1 + Dim.top * 2);
        Style.Set(Styles.Margin, Dim.bottom * 4);
        // 
        var layout = _layout = new HorizontalGrid();
        var hL0 = new HorizontalLayout();
        var hL1 = new HorizontalLayout(true);

        hL0.AddChild(new Label(title));
        // add toggle button to hl1
        hL1.AddChild(new ToggleButton(true, onToggleClicked));

        layout.AddChild(hL0);
        layout.AddChild(hL1);
        AddChild(layout);
      }
    }

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
