using System;

namespace GUIPanels
{

  public struct SliderSettings
  {
    public bool UseCustomColors;
    public Col Active, Filled;
    public float Height, MaxWidth;
    public static SliderSettings Default
    {
      get
      {
        return new SliderSettings()
        {
          UseCustomColors = false,
          Active = Col.white,
          Filled = Col.black,
          Height = 11,
          MaxWidth = 9999f
        };
      }
    }
  }
  public class SliderOld : Parameter
  {
    string _name;
    Action<float> _setValueCallback;
    Func<float> _getValueCallback;
    float _value { get { return _getValueCallback(); } }
    float _min = 0;
    float _max = 1;
    Style _style;
    SliderSettings _settings = SliderSettings.Default;
    public SliderOld(string name, Func<float> getValueCallback, Action<float> setValueCallback, float min = 0, float max = 1)
    {
      _name = name;
      _setValueCallback = setValueCallback;
      _getValueCallback = getValueCallback;
      _min = min;
      _max = max;
    }

    public SliderOld(string name, Func<float> getValueCallback, Action<float> setValueCallback, float min, float max, SliderSettings settings)
    {
      _settings = settings;
      _name = name;
      _setValueCallback = setValueCallback;
      _getValueCallback = getValueCallback;
      _min = min;
      _max = max;
    }

    public override void UpdateStyle()
    {
      _style = Utils.MakeStyle();
      _style.FontColor = Owner.TextColor;
      _style.FontSize = Owner.TextSize;
    }

    Vec2 _currentPosition = new Vec2(0, 0);
    public override float Width
    {
      get { return _settings.MaxWidth < base.Width ? _settings.MaxWidth : base.Width; }
      set { base.Width = value; }
    }

    Rectangle Rect
    {
      get
      {
        return new Rectangle(Position.x, Position.y, Width, Height);
      }
    }
    Rectangle TextRect { get { return new Rectangle(Position.x, Position.y, Width, Owner.TextSize); } }
    float SliderHeight { get { return _settings.Height; } }
    Col SliderColorFill { get { return _settings.UseCustomColors ? _settings.Filled : Owner.Style.SecondaryColor; } }
    Col SliderColorActive { get { return _settings.UseCustomColors ? _settings.Active : Owner.Style.PrimaryColor; } }

    float SliderStartY { get { return Owner.TextSize + Owner.PaddingLine; } }
    Rectangle SliderRect
    {
      get
      {
        return new Rectangle(Position.x, Position.y + SliderStartY, Width, SliderHeight);
      }
    }
    public override float Height
    {
      get
      {
        return SliderStartY + SliderHeight;
      }
    }
    Func<float, float> clamp01 = x => x < 0 ? 0 : x > 1 ? 1 : x;

    bool _mouseDown = false;
    public override void Repaint()
    {

      // 
      // handle mouse events
      var mousePosition = Utils.MousePosition();
      var sliderRect = Rect;
      if (sliderRect.Contains(mousePosition) && Utils.GetMouseDown())
      {
        _mouseDown = true;
      }

      if (_mouseDown)
      {
        float x = (mousePosition.x - sliderRect.x) / sliderRect.width;
        // Clamp
        x = clamp01(x);
        _setValueCallback(x * (_max - _min));
        _mouseDown = Utils.GetMouseButton();
      }

      Draw();
    }

    void Draw()
    {
      Rendering.DrawText(TextRect, string.Format("{0}: {1:0.00}",
      _name, _value), _style);
      var sliderRect = SliderRect;
      Rendering.DrawRect(sliderRect, SliderColorFill);
      Rendering.DrawRect(new Rectangle(sliderRect.x, sliderRect.y,
        clamp01(_value / (_max - _min)) * sliderRect.width, sliderRect.height),
        SliderColorActive);
    }
  }
}
