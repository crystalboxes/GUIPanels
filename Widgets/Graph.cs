namespace GUIPanels
{

  public enum GraphStyle
  {
    Spectrum, Waveform, MovingGraph
  }
  public class Graph : EmptySpace
  {
    public Outline WaveformLine = new Outline()
    {
      Color = Col.black,
      Width = 1
    };
    public Outline Line = new Outline()
    {
      Color = Col.white,
      Width = 1
    };
    float _min, _max;
    float[] _buffer;
    GraphStyle _style;
    float _height;
    public Graph(string title, float[] buffer, float min, float max, float width = 150, float height = 50) : base(width, height)
    {
      _height = height;
      _min = min; _max = max;
      _buffer = buffer;
    }
    public Graph SetGraphStyle(GraphStyle style)
    {
      _style = style;
      return this;
    }

    protected override void Render()
    {
      base.Render();

      var box = ContentBox;
      if (_style == GraphStyle.Waveform)
      {
        // draw line
        var pos = box.Position;
        pos.y += box.height * 0.5f;
        var pos2 = pos + new Vec2(box.width, 0);
        Rendering.DrawLine(pos, pos2, WaveformLine.Width, WaveformLine.Color);
      }

      if (_style == GraphStyle.MovingGraph || _style == GraphStyle.Waveform)
      {
        for (int x = 0; x < _buffer.Length - 1; x++)
        {
          float xPos0 = box.width * x / (float)_buffer.Length;
          float xPos1 = box.width * (x + 1) / (float)_buffer.Length;
          float v0 = Utils.Map(_buffer[x], _min, _max, 0, 1, true) * _height;
          float v1 = Utils.Map(_buffer[x + 1], _min, _max, 0, 1, true) * _height;

          float x0 = (box.Position.x + xPos0);
          float x1 = (box.Position.x + xPos1);
          float y0 = (box.Position.y + (_height - v0));
          float y1 = (box.Position.y + (_height - v1));

          Rendering.DrawLine(
            new Vec2(x0, y0), new Vec2(x1, y1), Line.Width, Line.Color);
        }
      }
      else
      {
        float ybase = Aspect.Adjust(box.y + _height);
        Rendering.BeginTriangleShape(Line.Color);
        for (int i = 0; i < _buffer.Length - 1; i++)
        {
          float xPos0 = box.width * i / (float)_buffer.Length;
          float xPos1 = box.width * (i + 1) / (float)_buffer.Length;
          float v0 = Utils.Map(_buffer[i], _min, _max, 0, 1, true) * _height;
          float v1 = Utils.Map(_buffer[i + 1], _min, _max, 0, 1, true) * _height;
          //
          float x0 = Aspect.Adjust(box.Position.x + xPos0);
          float x1 = Aspect.Adjust(box.Position.x + xPos1);
          float y0 = Aspect.Adjust(box.Position.y + (_height - v0));
          float y1 = Aspect.Adjust(box.Position.y + (_height - v1));

          Rendering.AddVertex(x0, y0);
          Rendering.AddVertex(x1, ybase);
          Rendering.AddVertex(x0, ybase);


          Rendering.AddVertex(x0, y0);
          Rendering.AddVertex(x1, y1);
          Rendering.AddVertex(x1, ybase);
        }
        Rendering.EndShape();
      }
    }
  }
}
