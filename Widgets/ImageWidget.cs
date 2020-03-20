using System;

namespace GUIPanels
{
  public class ImageWidget : EmptySpace
  {
    Texture _tex;
    bool _fillsContentBox;

    protected Rectangle ImageBox
    {
      get { return _fillsContentBox ? ContentBox : new Rectangle(ContentPosition.x, ContentPosition.y, ImageWidth, ImageHeight); }
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
    Func<Col> _setValueCallback;
    public ImageSampler(Texture texture, Func<Col> setValueCallback, Func<Vec2> getSamplerPostionCallback, Action<Vec2> setSamplerPostionCallback, float width = 0, float height = 0) : base(texture, width, height)
    {
      _valueComponent = new ValueComponent<Vec2>(getSamplerPostionCallback, setSamplerPostionCallback);
      _setValueCallback = setValueCallback;
    }

    protected override void Render()
    {
      base.Render();
      var box = ImageBox;

      const float emptySpace = 5;

      var pt = new Vec2(Value.x * box.width + box.x, Value.y * box.height + box.y);
      Rendering.DrawLine(new Vec2(pt.x, box.y), new Vec2(pt.x, pt.y - emptySpace), LinesWidth, LinesColor);
      Rendering.DrawLine(new Vec2(pt.x, box.y + box.height), new Vec2(pt.x, pt.y + emptySpace), LinesWidth, LinesColor);

      Rendering.DrawLine(new Vec2(box.x, pt.y), new Vec2(pt.x - emptySpace, pt.y), LinesWidth, LinesColor);
      Rendering.DrawLine(new Vec2(box.x + box.width, pt.y), new Vec2(pt.x + emptySpace, pt.y), LinesWidth, LinesColor);
    }

    // TODO dragging
    
  }
}