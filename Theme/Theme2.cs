namespace GUIPanels
{
  public class Theme2 : DefaultTheme
  {
    public float WindowPanelWidth { get { return 130; } }
    public override Col FontColor { get { return PrimaryColor; } }
    public override Col PrimaryColor { get { return new Col(1, 1, 1, 200f / 255f); } }
    public Col HoveredColor { get { return Col.white; } }
    public Col HoveredOutline { get { return new Col(1, 1, 1, 100f / 255f); } }
    public override Col SecondaryColor { get { return BackColor; } }
    public Col BackColor { get { return new Col(0, 0, 0, 100 / 255f); } }
    public float BasePadding { get { return 2; } }
    public float WidgetSpacing { get { return 1; } }
    public Theme2() : base()
    {
      System.Action<Widget> SetWidgetOutline = x =>
      {
        x.Style.Hovered
            .Outline(1, HoveredOutline)
          .Owner.Style.Clicked
            .Outline(1, HoveredOutline);
      };

      Add<LayoutBase>(x => { });
      Add<HeaderWidget>(x => { });

      Add<NumberDialer>(x =>
      {
        x.Style.Font(FontSmall);
        var s = x as NumberDialer;
        s.LabelStyle
          .Font(FontSmall)
          .Background(BackColor);
      });

      Add<SortableList>(x =>
      {
        var z = x as SortableList;
        z.Style
          .Padding(1);
        z.ItemStyle
          .Background(PrimaryColor)
          .Padding(2.5f, 0)
          .Margin(0.5f, 0.0f)
          .FontSize(FontMedium.Size)
          .FontColor(SecondaryColor);
      });

      Add<ToggleMatrix>(x =>
      {
        var z = x as ToggleMatrix;
        z.ToggleStyle
          .Padding(0)
          .Margin(0.5f);
        foreach (var t in z.Toggles)
        {
          t.ToggleButton.Style.Margin(0).Padding(0);
          t.SetToggleSize(FontLarge.Size);
        }
      });

      Add<Button>(x =>
      {
        x.Style
          .Padding(WidgetSpacing, 0)
          .FontSize(FontSizeSmall)
          .Background(SecondaryColor)
          .FontColor(PrimaryColor)
        .Clicked
          .Background(PrimaryColor)
          .FontColor(SecondaryColor);
        SetWidgetOutline(x);
      });

      Add<Graph>(x =>
      {
        x.Style.Background(BackColor);
        var z = x as Graph;
        z.Line.Color = PrimaryColor;
        z.WaveformLine.Color = SecondaryColor;
      });

      Add<Spacer>(x =>
      {
        (x as Spacer).Height = 0.5f;
        x.Style
          .Margin(WidgetSpacing, 0)
          .Background(PrimaryColor);
      });

      var sliderStyle = GetAction<Slider>();
      Add<Slider>(x =>
      {
        sliderStyle(x);
        var slider = x as Slider;
        slider.SliderHeight = 10f;
        slider.BringLabelToBottom();
        SetWidgetOutline(slider.InactiveBar);
      });

      var rangeSliderStyle = GetAction<RangeSlider>();
      Add<RangeSlider>(x =>
      {
        rangeSliderStyle(x);
        var rangeSlider = x as RangeSlider;
        rangeSlider.BringLabelToBottom();
        SetWidgetOutline(rangeSlider.InactiveBar);
      });

      var textFieldStyle = GetAction<TextField>();
      Add<TextField>(h =>
      {
        var c = h as TextField;
        textFieldStyle(h);
        c.TextFieldBox.Style.Background(SecondaryColor);
        c.PrimaryColor = PrimaryColor;
        c.SecondaryColor = SecondaryColor;
        c.TextFieldLabel.Style.SetHidden();
      });

      Add<VerticalSlider>(h =>
      {
        var c = h as VerticalSlider;
        c.InactiveBar.Style.Set(Styles.BackgroundColor, SecondaryColor);
        c.ActiveBar.Style.Set(Styles.BackgroundColor, PrimaryColor);
        SetWidgetOutline(c.SliderBar);
        c.Style.Font(FontSmall);
      });

      Add<SelectVertical>(a =>
      {
        var c = a as SelectVertical;
        c.Container.SetChildStyle = x => x.Style.Set(Styles.Margin, Dim.bottom * WidgetSpacing * 2);
      });

      var windowPanelStyle = GetAction<WindowPanel>();
      Add<WindowPanel>(x =>
      {
        windowPanelStyle(x);
        var c = x as WindowPanel;
        c.Style
          .Set<float>(Styles.Width, WindowPanelWidth)
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
            a.Style.Margin(WidgetSpacing, 0);
          });
      });

      Add<ToggleBase>(x =>
      {
        var c = x as ToggleBase;
        c.ToggleButton.Style
          .Border(0, Col.black);

        c.PrimaryColor = PrimaryColor;
        c.SecondaryColor = BackColor;
        SetWidgetOutline(c.ToggleButton);
      });

      var radioToggleStyle = GetAction<RadioToggle>();
      Add<RadioToggle>(x =>
      {
        if (radioToggleStyle != null)
        {
          radioToggleStyle(x);
        }
        var rt = x as RadioToggle;
        rt.DrawHoveredOutLine = true;
        rt.Outline = new Outline()
        {
          Width = 1,
          Color = HoveredOutline
        };
      });

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
          x.Style
            .Font(FontSizeSmall, PrimaryColor)
            .Hovered
              .FontColor(HoveredColor)
              .Owner.Style
            .Clicked
              .FontColor(HoveredColor);
        }, t);
      }
    }
  }
}
