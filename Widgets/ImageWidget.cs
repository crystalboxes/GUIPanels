using System;

namespace GUIPanels
{
  public class ImageWidget : EmptySpace
  {
    protected Texture _tex;
    bool _fillsContentBox;

    protected Rectangle ImageBox
    {
      get
      {
        if (_fillsContentBox) { return ContentBox; }
        else
        {
          return new Rectangle(ContentPosition.x, ContentPosition.y, ImageWidth, ImageHeight);
        }
      }
    }
    protected override float ContentHeight
    {
      get { return _fillsContentBox ? _tex.Height * ContentWidth / _tex.Width : base.ContentHeight; }
    }
    protected float ImageWidth { get; set; }
    protected float ImageHeight { get; set; }
    public ImageWidget(Texture texture, float width = 0, float height = 0) : base(width, height)
    {
      ImageWidth = width;
      ImageHeight = height;
      if (width < float.Epsilon)
      {
        ImageWidth = texture.Width;
      }
      if (height < float.Epsilon)
      {
        ImageHeight = texture.Height;
      }

      _tex = texture;
      SetFillContentBox();
    }

    public ImageWidget SetFillContentBox(bool value = true)
    {
      _fillsContentBox = value;
      return this;
    }

    protected override void Render()
    {
      base.Render();
      Rendering.DrawTexture(ImageBox, _tex);
    }
  }

  public class ImageSampler : ImageWidget
  {
    public Col SampledColor { get { return _sampledColor; } }
    Col _sampledColor;

    public Col LinesColor = Col.white;
    public float LinesWidth = 1f;
    Vec2 Value
    {
      get { return _valueComponent.Value; }
      set { _valueComponent.Value = value; }
    }
    ValueComponent<Vec2> _valueComponent;
    Action<Col> _setValueCallback;
    Vec2 _pos;
    public ImageSampler(Texture texture,
     float width = 0, float height = 0, Action<Col> setValueCallback = null,
     Func<Vec2> getSamplerPostionCallback = null, Action<Vec2> setSamplerPostionCallback = null) : base(texture, width, height)
    {
      if (getSamplerPostionCallback == null)
      {
        getSamplerPostionCallback = () => _pos;
      }
      if (setSamplerPostionCallback == null)
      {
        setSamplerPostionCallback = x => _pos = x;
      }
      _valueComponent = new ValueComponent<Vec2>(getSamplerPostionCallback, setSamplerPostionCallback);
      _setValueCallback = setValueCallback;
    }

    protected override void Render()
    {
      base.Render();
      var box = ImageBox;

      const float emptySpace = 3;

      var pt = new Vec2(Value.x * box.width + box.x, Value.y * box.height + box.y);

      Rendering.DrawLine(new Vec2(pt.x, box.y),
        new Vec2(pt.x, pt.y - emptySpace < box.y ? box.y : pt.y - emptySpace), LinesWidth, LinesColor);
      Rendering.DrawLine(new Vec2(box.x, pt.y),
        new Vec2(pt.x - emptySpace < box.x ? box.x : pt.x - emptySpace, pt.y), LinesWidth, LinesColor);

      Rendering.DrawLine(new Vec2(pt.x, box.y + box.height),
        new Vec2(pt.x,
          pt.y + emptySpace > box.y + box.height
            ? box.y + box.height
            : pt.y + emptySpace
        ), LinesWidth, LinesColor);

      Rendering.DrawLine(new Vec2(box.x + box.width, pt.y),
        new Vec2(
          pt.x + emptySpace > box.x + box.width
            ? box.x + box.width
            : pt.x + emptySpace, pt.y
        ), LinesWidth, LinesColor);
    }

    bool _isDragging = false;
    protected override void OnDraggingStart(float x, float y)
    {
      base.OnDraggingStart(x, y);
      _isDragging = true;
    }
    protected override void OnMouseUp()
    {
      _isDragging = false;
    }

    protected override void OnUpdate()
    {
      if (_isDragging)
      {
        var mousePos = Utils.MousePosition();
        var pos = ContentPosition;
        var uv = (mousePos - pos) / new Vec2(ImageWidth, ImageHeight);
        uv.x = Utils.Clamp01(uv.x);
        uv.y = Utils.Clamp01(uv.y);

        _sampledColor = _tex.GetPixel((int)(uv.x * _tex.Width), (int)((1 - uv.y) * _tex.Height));
        if (_setValueCallback != null)
        {
          _setValueCallback(_sampledColor);
        }
        Value = uv;
      }
    }
  }
}