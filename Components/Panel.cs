namespace GUIPanels
{
  public class PanelSettings : BasePanelCollapsibleSettings
  {
    public Col BackgroundColor = new Col(1, 1, 1, 0.3f);
  }

  public class Panel : BasePanelCollapsible
  {
    string _name;
    PanelSettings _panelSettings;

    protected override string Name
    {
      get { return _name; }
    }

    public Panel(PanelSettings settings, string name = "") : base(settings)
    {
      _name = name;
      _panelSettings = settings;
    }


    public void SetPosition(float x, float y)
    {
      Position = new Vec2(x, y);
    }


    public override void Draw()
    {
      // Need to update position
      HandleDragging();
      base.Draw();
    }


    protected override float StartY
    {
      get { return base.StartY + PaddingTop; }
    }

    public override void Add(IParameter param)
    {
      base.Add(param);
      param.UpdateStyle();
    }


    protected override void DrawWindowBox()
    {
      Rendering.DrawRect(WindowBoxRect, _panelSettings.BackgroundColor);
    }


    bool _mouseDown;
    Vec2 _offset;

    void HandleDragging()
    {
      var hr = HeaderRect;
      var mp = Utils.MousePosition();
      if (hr.Contains(mp) && Utils.GetMouseDown() && !_mouseDown)
      {
        _offset = Position - mp;
        _mouseDown = true;
      }

      if (_mouseDown)
      {
        Position = mp + _offset;
        _mouseDown = Utils.GetMouseButton();
      }
    }
  }
}