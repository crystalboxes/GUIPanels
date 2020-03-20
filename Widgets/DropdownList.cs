namespace GUIPanels
{
  public class TriangleComponent : EmptySpace
  {
    public Col Color;
    public TriangleComponent(float width, float height, Col color) : base(width, height)
    {
      Color = color;
    }
    protected override void Render()
    {
      Rendering.DrawTriangle(ContentBox, Color);
    }
  }
  public class DropdownList : VerticalLayout
  {
    public TriangleComponent Triangle { get { return _triangle; } }
    public Label ActiveLabel { get { return _activeLabel; } }
    public VerticalLayout OpenedBox { get { return _opened; } }
    TriangleComponent _triangle;
    VerticalLayout _opened;
    Label _activeLabel;
    string[] _options;

    public int CurrentIndex { get { return System.Array.IndexOf(_options, _activeLabel.Title); } }

    public class ClickableLable : HorizontalGrid
    {
      System.Action<string> _onClickCallback;
      // public Col HoveredColor { get; set; }
      Col _prevColor;
      Label _label;
      public ClickableLable(string title, System.Action<string> onClickCallback) : base()
      {

        _onClickCallback = onClickCallback;
        _label = new Label(title);
        AddChild(_label);
      }
      protected override void OnClick()
      {
        base.OnClick();
        Utils.Event.OnClick.Use();
        _onClickCallback(_label.Title);
      }
      // protected override void OnUpdate()
      // {
      //   base.OnUpdate();
      //   if (Box.Contains(Utils.MousePosition()))
      //   {
      //     Style.Set<Col>(Styles.BackgroundColor, HoveredColor);
      //   }
      //   else
      //   {
      //     Style.Set<Col>(Styles.BackgroundColor, new Col(0, 0, 0, 0));
      //   }
      // }
    }

    public DropdownList(string[] options, int selectedIndex, float width = 100) : base(width)
    {
      _options = options;

      var grid = new HorizontalGrid();
      var left = new HorizontalLayout();
      var right = new HorizontalLayout(true);
      _activeLabel = new Label(options[selectedIndex]);
      left.Attach(_activeLabel);
      float height = Utils.CalcSize("Adsd", Style).y;
      _triangle = new TriangleComponent(height, height, Col.white);
      right.Attach(_triangle);
      grid.Attach(left, right);
      AddChild(grid);

      _opened = new VerticalLayout(InnerWidth);
      foreach (var option in options)
      {
        _opened.Attach(new ClickableLable(option, x =>
        {
          _activeLabel.Title = x;
          Toggle();
        }));
      }
    }
    public override void DeferRender()
    {
      base.DeferRender();

      if (_active)
      {
        var box = _activeLabel.Box;
        box.y += box.height;
        _opened.Style.Set<float>(Styles.Width, InnerWidth);
        _opened.Position = box.Position;
        _opened.Draw();
      }
    }
    bool _active = false;
    protected override void OnMouseUp()
    {
      base.OnMouseUp();
      var pos = Utils.MousePosition();
      if (Box.Contains(pos))
      {
        Toggle();
      }
    }
    void Toggle()
    {
      _active = !_active;
      _triangle.Style.Set(Styles.Hidden, _active);
    }
  }
}
