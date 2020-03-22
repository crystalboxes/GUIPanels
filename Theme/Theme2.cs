namespace GUIPanels
{
  public class Theme2 : DefaultTheme
  {
    public override Col FontColor { get { return PrimaryColor; } }
    public override Col PrimaryColor { get { return new Col(1, 1, 1, 200f / 255f); } }
    public override Col SecondaryColor { get { return BackColor; } }
    public Col BackColor { get { return new Col(0, 0, 0, 100 / 255f); } }
    public float BasePadding { get { return 2; } }
    public Theme2() : base()
    {
      Add<LayoutBase>(x => { });
      Add<HeaderWidget>(x => { });

      foreach (var t in new System.Type[] {
          typeof(Slider), typeof(VerticalSlider),
          typeof(SelectHorizontal), typeof(SelectVertical),
          typeof(VerticalSlider), typeof(ToggleButton),
          typeof(RadioToggle), typeof(Toggle),
          typeof(ToggleButton), typeof(RangeSlider)
        })
      {
        var style = GetAction(t);
        Add(x =>
        {
          if (style != null) { style(x); }
          x.Style.Font(FontSizeSmall, PrimaryColor);
        }, t);
      }

      Add<Spacer>(x =>
      {
        (x as Spacer).Height = 1;
        x.Style
          .Margin(1, 0)
          .Background(PrimaryColor);
      });

      var sliderStyle = GetAction<Slider>();
      Add<Slider>(x =>
      {
        sliderStyle(x);
        (x as Slider).BringLabelToBottom();
      });

      var rangeSliderStyle = GetAction<RangeSlider>();
      Add<RangeSlider>(x =>
      {
        rangeSliderStyle(x);
        (x as RangeSlider).BringLabelToBottom();
      });

      Add<VerticalSlider>(h =>
      {
        var c = h as VerticalSlider;
        c.InactiveBar.Style.Set(Styles.BackgroundColor, SecondaryColor);
        c.ActiveBar.Style.Set(Styles.BackgroundColor, PrimaryColor);
      });

      var windowPanelStyle = GetAction<WindowPanel>();
      Add<WindowPanel>(x =>
      {
        windowPanelStyle(x);
        var c = x as WindowPanel;
        c.Style
          .Set<float>(Styles.Width, 143)
          .Font(FontSizeLarge, PrimaryColor);

        c.HeaderOffset = 0;
        c.Header.Style
          .Background(BackColor)
          .Padding(BasePadding, BasePadding, 0);
        c.Container.Style
          .Background(BackColor)
          .Border(0, Col.black)
          .Padding(0, BasePadding, BasePadding)
          .Owner
          .ApplyChildStyle(a =>
          {
            a.Style.Margin(1, 0);
          });
      });

      Add<ToggleBase>(x =>
      {
        var c = x as ToggleBase;
        c.PrimaryColor = PrimaryColor;
        c.SecondaryColor = BackColor;
      });

    }
  }
}
