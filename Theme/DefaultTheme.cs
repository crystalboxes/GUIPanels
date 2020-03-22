using System;
namespace GUIPanels
{

  public class DefaultTheme : Theme
  {
    public override Col PrimaryColor { get { return Col.white; } }
    public override Col SecondaryColor { get { return Col.black; } }
    public override float FontSizeLarge { get { return 10; } }
    public override float FontSizeSmall { get { return 7; } }
    public override float FontSizeMedium { get { return 8; } }
    public override Col FontColor { get { return SecondaryColor; } }
    public DefaultTheme() : base()
    {

      Add<LayoutBase>(x =>
      {
        x.Style
          .Font(10, SecondaryColor);
      });

      Action<Widget> hh = c =>
      {
        c.Style.Set(Styles.BackgroundColor, new Col(0.5f, 0.5f, 0.5f, 1));
        c.Style.Set(Styles.Height, 12f);
        c.Style.Set(Styles.Padding, Dim.left * 2 + Dim.bottom * 1 + Dim.top * 2);
        c.Style.Set(Styles.Margin, Dim.bottom * 4);
      };
      Add<HeaderWidget>(hh);
      Add<HeaderWidget2>(hh);

      Add<DropdownList>(h =>
      {
        var c = h as DropdownList;
        c.Style.Set<Col>(Styles.BackgroundColor, SecondaryColor);
        c.ActiveLabel.Style.Set<Col>(Styles.FontColor, PrimaryColor);
        c.Triangle.Style.Set<Dim>(Styles.Padding, new Dim(3f));
        c.Triangle.Color = PrimaryColor;
        c.OpenedBox.Style.Set(Styles.BackgroundColor, PrimaryColor);
      });

      Add<Button>(h =>
      {
        h.Style
          .Background(PrimaryColor)
          .FontColor(SecondaryColor)
        .Hovered
          .Background(SecondaryColor)
          .FontColor(PrimaryColor);
      });

      Add<ColorPicker>(h =>
      {
        var c = h as ColorPicker;
        c.HandleColor = PrimaryColor;
        c.HeaderColor = new Col(.5f, .5f, .5f, 1);
        c.ColorDisplayWidth = 10f;
        c.HueSlider.Style.Set(Styles.Height, 10f);

        c.PickerhandleSize = 10;
        c.HueSliderHandleWidth = 6;
        c.HandleBorderWidth = 1;
      });

      Add<RadioToggle>(h =>
      {
        var c = h as RadioToggle;
        c.PrimaryColor = SecondaryColor;
        c.SecondaryColor = PrimaryColor;
        c.CheckedRadiusRatio = 0.5f;
      });

      Add<RotarySlider>(h =>
      {
        var c = h as RotarySlider;
        c.Radius = 25f;
        c.Width = 9f;
        c.EmptyColor = SecondaryColor;
        c.FilledColor = PrimaryColor;
      });

      Add<CircleSlider>(h =>
      {
        var c = h as CircleSlider;
        c.Radius = 25f;
        c.EmptyColor = SecondaryColor;
        c.FilledColor = PrimaryColor;
      });

      Add<VerticalSlider>(h =>
      {
        var c = h as VerticalSlider;
        c.InactiveBar.Style.Set(Styles.BackgroundColor, SecondaryColor);
        c.ActiveBar.Style.Set(Styles.BackgroundColor, PrimaryColor);
        c.Width = 20f;
      });

      Add<Slider>(h =>
      {
        var c = h as Slider;
        c.InactiveBar.Style.Set(Styles.BackgroundColor, SecondaryColor);
        c.ActiveBar.Style.Set(Styles.BackgroundColor, PrimaryColor);
        c.SliderHeight = 10f;
      });

      Add<RangeSlider>(x =>
      {
        var c = x as RangeSlider;
        c.SetHeight(10);
        c.InactiveBar.Style.Set(Styles.BackgroundColor, SecondaryColor);
        c.ActiveBar.Style.Set(Styles.BackgroundColor, PrimaryColor);
      });

      Add<TextField>(h =>
      {
        var c = h as TextField;
        c.PrimaryColor = PrimaryColor;
        c.SecondaryColor = SecondaryColor;
      });

      Add<DropdownList.ClickableLable>(a =>
      {
        a.Style.Hovered
          .Background(new Col(0.5f, 0.5f, 0.5f, 0.45f));
      });

      Add<SelectVertical>(a =>
      {
        var c = a as SelectVertical;
        c.Container.SetChildStyle = x => x.Style.Set(Styles.Margin, Dim.bottom * 3);
      });

      Add<HeaderWidget2.ToggleButton>(x =>
      {
        var c = x as HeaderWidget2.ToggleButton;
        c.Style.Set(Styles.Margin, Dim.right * 2);
      });

      Add<WindowPanel>(w =>
      {
        var c = w as WindowPanel;
        c.Container.Style.Set(Styles.BackgroundColor, new Col(1, 1, 1, .55f));
        c.HeaderOffset = 4;
        c.Container.Style.Set("background-color", new Col(1, 1, 1, .4f));
        c.Container.Style.Set(Styles.Border, new Dim(1));
        c.Container.Style.Set(Styles.BorderColor, new Col(1, 1, 1, .22f));
        c.Container.Style.Set(Styles.Padding, Dim.bottom * 5 + Dim.left * 3 + Dim.right * 4);
      });
    }
  }
}