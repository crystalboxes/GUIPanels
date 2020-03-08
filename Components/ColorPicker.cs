using System;

namespace GUIPanels
{
  public struct ColorPickerSettings
  {
    public float TextBoxPadding, ColorPreviewWidthPercent,
      PickerHeight, HueSliderHeight, HueSliderHandleWidth,
      HueSliderVerticalOffset, HandleBorderWidth, PickerHandleSize;
    public Col HandleColor;
    public static ColorPickerSettings Default
    {
      get
      {
        return new ColorPickerSettings()
        {
          TextBoxPadding = 2,
          ColorPreviewWidthPercent = 0.13f,
          HandleColor = Col.white,
          PickerHeight = 150,
          HueSliderHeight = 13,
          HueSliderVerticalOffset = 4,
          HueSliderHandleWidth = 7,
          HandleBorderWidth = 1,
          PickerHandleSize = 9,
        };
      }
    }
  }
  public class ColorPicker : Parameter
  {
    float TextRectHeight
    {
      get { return Owner.TextSize + TextBoxPadding * 2; }
    }

    Rectangle TextRect
    {
      get { return new Rectangle(Position.x, Position.y, Width, Owner.TextSize + TextBoxPadding * 2); }
    }

    string _name;
    Action<Col> _setValueCallback;
    Func<Col> _getValueCallback;
    Col _value { get { return _getValueCallback(); } }
    ColorPickerSettings _settings;

    bool _collapsed = true;
    bool _initialized = false;
    public Vec3 HSV;

    Col TargetColor
    {
      get { return _getValueCallback(); }
      set
      {
        // _value = value;
        _setValueCallback(value);
        Utils.RGBToHSV(value, out HSV.x, out HSV.y, out HSV.z);
      }
    }

    public ColorPicker(string name, Func<Col> getValueCallback, Action<Col> setValueCallback, ColorPickerSettings settings)
    {
      _name = name;
      _setValueCallback = setValueCallback;
      _getValueCallback = getValueCallback;
      TargetColor = _getValueCallback();// value;
      _settings = settings;
      Init();
    }
    public ColorPicker(string name, Func<Col> getValueCallback, Action<Col> setValueCallback)
    {
      _name = name;
      _getValueCallback = getValueCallback;
      _setValueCallback = setValueCallback;
      TargetColor = _getValueCallback();// value;
      _settings = ColorPickerSettings.Default;
      Init();
    }

    const int _pickingTextureSize = 64, _hueTextureSize = 32;
    Texture _pickingTexture, _hueTexture, _displayTexture;
    void Init()
    {
      // hue texture and picker texture
      _pickingTexture = Utils.MakeTexture(_pickingTextureSize, _pickingTextureSize);
      _hueTexture = Utils.MakeTexture(_hueTextureSize, 1);
      _displayTexture = Utils.MakeTexture(1, 1);

      // generate hue texture
      for (var x = 0; x < _hueTextureSize; x++)
      {
        _hueTexture.SetPixel(x, 0, Utils.HSVToRGB(x / (float)_hueTextureSize, 1, 1));
      }
      _hueTexture.Apply();
      Update();
      _initialized = true;
    }

    void Update()
    {
      float div = 1.0f / (float)_pickingTextureSize;

      for (var x = 0; x < _pickingTextureSize; x++)
      {
        float fx = x * div;
        for (var y = 0; y < _pickingTextureSize; y++)
        {
          float fy = (y * div);
          _pickingTexture.SetPixel(x, y, Utils.HSVToRGB(HSV.x, fx, fy));
        }
      }

      _pickingTexture.Apply();

      _displayTexture.SetPixel(0, 0, TargetColor);
      _displayTexture.Apply();
    }


    public void UpdateSaturationValue(float h, float s, float v)
    {
      var col = Utils.HSVToRGB(h, s, v);
      TargetColor = col;
      HSV = new Vec3(h, s, v);

      _displayTexture.SetPixel(0, 0, TargetColor);
      _displayTexture.Apply();
    }


    public void FullUpdate(float h, float s, float v)
    {
      var col = Utils.HSVToRGB(h, s, v);
      TargetColor = col;
      HSV = new Vec3(h, s, v);
      Update();
    }


    bool _mouseDownHueSlider = false;
    bool _mouseDownPicker = false;

    void DrawTransparentRectangle(Rectangle rect, float borderWidth)
    {
      var handleColor = _settings.HandleColor;
      Rendering.DrawRect(new Rectangle(rect.x, rect.y, rect.width, borderWidth),
      handleColor);
      Rendering.DrawRect(new Rectangle(rect.x, rect.y, borderWidth, rect.height),
      handleColor);
      Rendering.DrawRect(new Rectangle(rect.x, rect.y + rect.height - borderWidth, rect.width, borderWidth),
       handleColor);
      Rendering.DrawRect(new Rectangle(rect.x + rect.width - borderWidth, rect.y, borderWidth, rect.height),
       handleColor);
    }


    const float _paddingX = 3;
    const float collapseButtonPaddingAmount = 0.65f;
    float TextBoxPadding { get { return _settings.TextBoxPadding; } }

    Style _collapseTextStyle = null;

    public override void Repaint()
    {
      // Update collapse state
      var textRect = TextRect;
      if (textRect.Contains(Utils.MousePosition()) && Owner.MouseUp && !_pickerMouseDown && !_hueSliderMouseDown)
      {
        _collapsed = !_collapsed;
      }

      // Draw text with color
      if (_collapsed)
      {
        Rendering.DrawRect(textRect, _value);
      }
      if (_collapseTextStyle == null)
      {
        _collapseTextStyle = Utils.MakeStyle();
        _collapseTextStyle.FontColor = Col.white;
        _collapseTextStyle.FontSize = collapseButtonPaddingAmount * textRect.height;
      }
      Rendering.DrawText(Utils.GetCollapseButtonRect(Position, textRect.height,
        collapseButtonPaddingAmount, Owner.TextSize, textRect.width), Utils.GetCollapseButtonText(_collapsed), _collapseTextStyle);

      textRect.y += TextBoxPadding;
      textRect.x += _paddingX;
      Rendering.DrawText(textRect, _name, Owner.Style);

      if (_collapsed)
      {
        return;
      }

      // Draw display color
      Draw(new Vec2(Position.x, PickerStartY), ref _settings);
    }

    float PickerStartY { get { return Position.y + TextRectHeight + Owner.PaddingLine; } }

    public override float Height
    {
      get
      {
        float h = TextRectHeight;
        if (!_collapsed)
        {
          h += Owner.PaddingLine
          + _settings.PickerHeight + _settings.HueSliderVerticalOffset;
        }
        return h;
      }
    }
    float DisplayColorWidth { get { return Width * _settings.ColorPreviewWidthPercent; } }
    bool _pickerMouseDown = false;
    bool _hueSliderMouseDown = false;
    static float Clamp01(float val)
    {
      return val < 0 ? 0 : val > 1 ? 1 : val;
    }

    public void Draw(Vec2 pos, ref ColorPickerSettings s)
    {
      var pickerRect = new Rectangle(pos.x + DisplayColorWidth, pos.y, Width - DisplayColorWidth, s.PickerHeight);

      Rendering.DrawTexture(new Rectangle(pos.x, pos.y, DisplayColorWidth, s.PickerHeight), _displayTexture);
      Rendering.DrawTexture(pickerRect, _pickingTexture);

      {
        var rect = new Rectangle(pickerRect.x + HSV.y * pickerRect.width, pos.y + (1 - HSV.z) * s.PickerHeight,
          s.PickerHandleSize, s.PickerHandleSize);
        rect.x -= s.PickerHandleSize * 0.5f;
        rect.y -= s.PickerHandleSize * 0.5f;

        if (rect.x + rect.width > pickerRect.x + pickerRect.width)
        {
          rect.x = pickerRect.x + pickerRect.width - rect.width;
        }

        if (rect.y + rect.height > pickerRect.y + pickerRect.height)
        {
          rect.y = pickerRect.y + pickerRect.height - rect.height;
        }

        if (rect.x < pickerRect.x)
        {
          rect.x = pickerRect.x;
        }

        if (rect.y < pickerRect.y)
        {
          rect.y = pickerRect.y;
        }
        DrawTransparentRectangle(rect, s.HandleBorderWidth);
      }

      pos.y += s.HueSliderVerticalOffset + s.PickerHeight;
      var hueSliderRect = new Rectangle(pos.x, pos.y, Width, s.HueSliderHeight);
      Rendering.DrawTexture(hueSliderRect, _hueTexture);
      {
        var rect = new Rectangle(pos.x + HSV.x * Width, pos.y,
        s.HueSliderHandleWidth, s.HueSliderHeight);
        rect.x -= s.HueSliderHandleWidth * 0.5f;
        if (rect.x + rect.width > hueSliderRect.x + hueSliderRect.width)
        {
          rect.x = hueSliderRect.x + hueSliderRect.width - s.HueSliderHandleWidth;
        }
        if (rect.x < hueSliderRect.x)
        {
          rect.x = hueSliderRect.x;
        }
        DrawTransparentRectangle(rect, s.HandleBorderWidth);
      }

      var mousePos = Utils.MousePosition();
      bool mouseDown = Utils.GetMouseDown();
      if (pickerRect.Contains(mousePos) && mouseDown)
      {
        _pickerMouseDown = true;
      }

      if (_pickerMouseDown)
      {
        var sat = Clamp01((mousePos.x - pickerRect.x) / pickerRect.width);
        var val = Clamp01(1 - (mousePos.y - pickerRect.y) / pickerRect.height);
        _pickerMouseDown = Utils.GetMouseButton();
        UpdateSaturationValue(HSV.x, sat, val);
      }

      if (hueSliderRect.Contains(mousePos) && mouseDown)
      {
        _hueSliderMouseDown = true;
      }

      if (_hueSliderMouseDown)
      {
        var hue = Clamp01((mousePos.x - hueSliderRect.x) / hueSliderRect.width);
        _hueSliderMouseDown = Utils.GetMouseButton();
        FullUpdate(hue, HSV.y, HSV.z);
      }
    }



    public override void UpdateStyle()
    {
    }
  }
}