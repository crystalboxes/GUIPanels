using System.Collections.Generic;

namespace GUIPanels
{
  public class Theme
  {
    static Theme _current;
    public static Theme Current
    {
      get
      {
        if (_current == null)
        {
          _current = new Theme();
        }
        return _current;
      }
      set { _current = value; }
    }

    public void Apply(Widget c)
    {
      if (_styles.ContainsKey(c.GetType()))
      {
        _styles[c.GetType()](c);
      }
    }
    public Theme()
    {
      Add<HeaderComponent>(h =>
      {
        var c = h as HeaderComponent;
        c.Style.Set(Styles.BackgroundColor, new Col(0.5f, 0.5f, 0.5f, 1));
        c.Style.Set(Styles.Height, 12f);
        c.Style.Set(Styles.Padding, Dim.left * 2 + Dim.bottom * 1 + Dim.top * 2);
        c.Style.Set(Styles.Margin, Dim.bottom * 4);
      });

      Add<DropdownList>(h =>
      {
        var c = h as DropdownList;
        c.Style.Set<Col>(Styles.BackgroundColor, Col.black);
        c.ActiveLabel.Style.Set<Col>(Styles.FontColor, Col.white);
        c.Triangle.Style.Set<Dim>(Styles.Padding, new Dim(3f));
        c.Triangle.Color = Col.white;
        c.OpenedBox.Style.Set(Styles.BackgroundColor, Col.white);
      });

      Add<Button>(h =>
      {
        var c = h as Button;
        c.ClickedColor = Col.black;
        c.ButtonColor = Col.white;
      });

      Add<ColorPicker>(h =>
      {
        var c = h as ColorPicker;
        c.HandleColor = Col.white;
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
        c.PrimaryColor = Col.black;
        c.SecondaryColor = Col.white;
        c.CheckedRadiusRatio = 0.5f;
      });

      Add<RotarySlider>(h =>
      {
        var c = h as RotarySlider;
        c.Radius = 25f;
        c.Width = 9f;
        c.EmptyColor = Col.black;
        c.FilledColor = Col.white;
      });

      Add<VerticalSlider>(h =>
      {
        var c = h as VerticalSlider;
        c.InactiveBar.Style.Set(Styles.BackgroundColor, Col.black);
        c.ActiveBar.Style.Set(Styles.BackgroundColor, Col.white);
        c.Width = 20f;
      });

      Add<Slider>(h =>
      {
        var c = h as Slider;
        c.InactiveBar.Style.Set(Styles.BackgroundColor, Col.black);
        c.ActiveBar.Style.Set(Styles.BackgroundColor, Col.white);
        c.SliderHeight = 10f;
      });

      Add<TextField>(h =>
      {
        var c = h as TextField;
        c.PrimaryColor = Col.white;
        c.SecondaryColor = Col.black;
      });

      Add<DropdownList.ClickableLable>(a =>
      {
        var c = a as DropdownList.ClickableLable;
        c.HoveredColor = new Col(0.5f, 0.5f, 0.5f, 0.45f);
      });

      Add<SelectVertical>(a =>
      {
        var c = a as SelectVertical;
        c.Container.SetChildStyle = x => x.Style.Set(Styles.Margin, Dim.bottom * 3);
      });

      Add<ToggleButton>(x =>
      {
        var c = x as ToggleButton;
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

    protected void Add<T>(System.Action<Widget> style)
    {
      _styles[typeof(T)] = style;
    }

    Dictionary<System.Type, System.Action<Widget>> _styles =
      new Dictionary<System.Type, System.Action<Widget>>();
  }
}
