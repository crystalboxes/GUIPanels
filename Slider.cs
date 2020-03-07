using System;

namespace GUIPanels
{

  public struct SliderSettings
  {
    public Col Active, Filled;
    public float Height, MaxWidth;
    public static SliderSettings Default
    {
      get
      {
        return new SliderSettings()
        {
          Active = Col.white,
          Filled = Col.black,
          Height = 16,
          MaxWidth = 9999f
        };
      }
    }
  }
  public class Slider : Parameter
  {
    string _name;
    Action<float> _callback;
    float _value = 0;
    float _min = 0;
    float _max = 1;
    Style _style;
    SliderSettings _settings = SliderSettings.Default;
    public Slider(string name, Action<float> callback, float value = 0, float min = 0, float max = 1)
    {
      _name = name;
      _callback = callback;
      _value = value;
      _min = min;
      _max = max;
    }
    public Slider(string name, Action<float> callback, SliderSettings settings, float value = 0, float min = 0, float max = 1)
    {
      _settings = settings;
      _name = name;
      _callback = callback;
      _value = value;
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
    Col SliderColorFill { get { return _settings.Filled; } }
    Col SliderColorActive { get { return _settings.Active; } }

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


    bool _mouseDown = false;
    public override void Repaint()
    {
      // 
      // handle mouse events
      var mousePosition = Utils.MousePosition();
      var sliderRect = Rect;
      if (sliderRect.Contains(mousePosition) && Utils.GetMouseButton())
      {
        _mouseDown = true;
      }

      if (_mouseDown)
      {
        float x = (mousePosition.x - sliderRect.x) / sliderRect.width;
        x = x < 0 ? 0 : x > 1 ? 1 : x;
        _value = x * (_max - _min);
        _callback(_value);
        _mouseDown = Utils.GetMouseButton();
      }

      Draw();
    }

    void Draw()
    {
      Renderer.Current.DrawText(TextRect, string.Format("{0}: {1:0.00}",
      _name, _value), _style);
      var sliderRect = SliderRect;
      Renderer.Current.DrawRect(sliderRect, SliderColorFill);
      Renderer.Current.DrawRect(new Rectangle(sliderRect.x, sliderRect.y,
        _value / (_max - _min) * sliderRect.width, sliderRect.height),
        SliderColorActive);
    }
  }
}