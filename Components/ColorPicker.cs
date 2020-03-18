using System;
namespace GUIPanels
{
  public class ColorPicker : VerticalLayout
  {
    public EmptySpace ColorDisplay { get { return _colorDisplayBlock; } }
    public EmptySpace Picker { get { return _pickerBlock; } }
    public VerticalLayout PickerLayout { get { return _pickerLayout; } }
    public EmptySpace HueSlider { get { return _hueSlider; } }
    public string Title { get; set; }
    public float PickerHeight { get; set; }
    public float ColorDisplayWidth
    {
      get { return _colorDisplayWidth; }
      set
      {
        ColorDisplay.Style.Set<float>(Styles.Width, value);
        Picker.Style.Set<float>(Styles.Width, InnerWidth - value);
        _colorDisplayWidth = value;
      }
    }
    public Col HandleColor { get; set; }
    public Col HeaderColor { get; set; }

    HeaderComponent _header;
    EmptySpace _pickerBlock;
    EmptySpace _hueSlider;
    EmptySpace _colorDisplayBlock;
    VerticalLayout _pickerLayout;
    ValueComponent<Col> _valueComponent;
    float _colorDisplayWidth = 20f;
    Vec3 HSV;

    Col _value { get { return _valueComponent.Value; } }
    Col TargetColor
    {
      get { return _valueComponent.Value; }
      set
      {
        _valueComponent.Value = value;
        Utils.RGBToHSV(value, out HSV.x, out HSV.y, out HSV.z);
        _colorDisplayBlock.Style.Set<Col>(Styles.BackgroundColor, value);
        if (_header != null && _header.IsCollapsed)
        {
          _header.Style.Set<Col>(Styles.BackgroundColor, TargetColor);
        }
      }
    }

    public ColorPicker(string title, Func<Col> getValueCallback = null,
    Action<Col> setValueCallback = null, float pickerHeight = 100) : base(100)
    {
      // Initialize texture here
      _colorDisplayWidth = 10f;
      Title = title;
      _valueComponent = new ValueComponent<Col>(getValueCallback, setValueCallback);
      _colorDisplayBlock = new EmptySpace(ColorDisplayWidth, pickerHeight);
      TargetColor = _valueComponent.Value;

      Init();


      // Inititialize texture end...
      _header = new HeaderComponent(Title, x =>
      {
        if (!x)
        {
          _header.Style.Set<Col>(Styles.BackgroundColor, TargetColor);
        }
        else
        {
          _header.Style.Set<Col>(Styles.BackgroundColor, HeaderColor);
        }
        _pickerLayout.Style.Set(Styles.Hidden, !x);
      });
      // vertical layout
      _pickerLayout = new VerticalLayout();
      var horizontalGrid = new HorizontalLayout();
      _colorDisplayBlock = new EmptySpace(ColorDisplayWidth, pickerHeight);
      _pickerBlock = new TextureWidget(_pickingTexture, InnerWidth - _colorDisplayWidth, pickerHeight);

      _colorDisplayBlock.Style.Set<Col>(Styles.BackgroundColor, TargetColor);
      _pickerBlock.Style.Set<Col>(Styles.BackgroundColor, new Col(0, 0, 0, 0));

      ColorDisplayWidth = _colorDisplayWidth;

      horizontalGrid.AddChild(_colorDisplayBlock);
      horizontalGrid.AddChild(_pickerBlock);

      _pickerLayout.AddChild(horizontalGrid);

      // add slider
      _hueSlider = new TextureWidget(_hueTexture, InnerWidth, 10);
      _hueSlider.Style.Set<Col>(Styles.BackgroundColor, Col.red);
      _pickerLayout.AddChild(_hueSlider);

      AddChild(_header);
      AddChild(_pickerLayout);
    }
    bool _pickerMouseDown = false, _hueSliderMouseDown = false;
    public float PickerhandleSize = 10,
       HueSliderHandleWidth = 6,
       HandleBorderWidth = 1;

    protected override void Render()
    {
      base.Render();
      if (_header.IsCollapsed)
      {
        return;
      }
      // Draw picker handle
      var pickerRect = _pickerBlock.Box;
      {
        var rect = new Rectangle(pickerRect.x + HSV.y * pickerRect.width, pickerRect.y + (1 - HSV.z) * pickerRect.height,
          PickerhandleSize, PickerhandleSize);
        rect.x -= PickerhandleSize * 0.5f;
        rect.y -= PickerhandleSize * 0.5f;

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
        DrawTransparentRectangle(rect, HandleBorderWidth);
      }
      // Draw hue slider handle
      var hueSliderRect = _hueSlider.Box;
      {
        var pos = hueSliderRect.Position;
        var rect = new Rectangle(pos.x + HSV.x * hueSliderRect.width, pos.y,
        HueSliderHandleWidth, hueSliderRect.height);
        rect.x -= HueSliderHandleWidth * 0.5f;
        if (rect.x + rect.width > hueSliderRect.x + hueSliderRect.width)
        {
          rect.x = hueSliderRect.x + hueSliderRect.width - HueSliderHandleWidth;
        }
        if (rect.x < hueSliderRect.x)
        {
          rect.x = hueSliderRect.x;
        }
        DrawTransparentRectangle(rect, HandleBorderWidth);
      }
      var mousePos = Utils.MousePosition();
      bool mouseDown = Utils.GetMouseDown();
      if (pickerRect.Contains(mousePos) && mouseDown)
      {
        _pickerMouseDown = true;
      }

      if (_pickerMouseDown)
      {
        var sat = Utils.Clamp01((mousePos.x - pickerRect.x) / pickerRect.width);
        var val = Utils.Clamp01(1 - (mousePos.y - pickerRect.y) / pickerRect.height);
        _pickerMouseDown = Utils.GetMouseButton();
        UpdateSaturationValue(HSV.x, sat, val);
      }

      if (hueSliderRect.Contains(mousePos) && mouseDown)
      {
        _hueSliderMouseDown = true;
      }

      if (_hueSliderMouseDown)
      {
        var hue = Utils.Clamp01((mousePos.x - hueSliderRect.x) / hueSliderRect.width);
        _hueSliderMouseDown = Utils.GetMouseButton();
        FullUpdate(hue, HSV.y, HSV.z);
      }

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

    void DrawTransparentRectangle(Rectangle rect, float borderWidth)
    {
      var handleColor = HandleColor;
      Rendering.DrawRect(new Rectangle(rect.x, rect.y, rect.width, borderWidth),
      handleColor);
      Rendering.DrawRect(new Rectangle(rect.x, rect.y, borderWidth, rect.height),
      handleColor);
      Rendering.DrawRect(new Rectangle(rect.x, rect.y + rect.height - borderWidth, rect.width, borderWidth),
       handleColor);
      Rendering.DrawRect(new Rectangle(rect.x + rect.width - borderWidth, rect.y, borderWidth, rect.height),
       handleColor);
    }

  }
}