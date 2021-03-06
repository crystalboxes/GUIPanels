namespace GUIPanels
{
  public abstract class Texture
  {
    public abstract void SetPixel(int x, int y, Col color);
    public abstract void Apply();
    public abstract float Width { get; }
    public abstract float Height { get; }
    public abstract Col GetPixel(int x, int y);
  }
  public static partial class Utils
  {
    public class Event
    {
      public void Use()
      {
        _used = true;
      }
      public bool Used { get { return _used; } }
      bool _used;
      static Event _onClickEvent;
      static int _previousFrame;
      public static Event OnClick
      {
        get
        {
          if (_onClickEvent == null || UnityEngine.Time.frameCount != _previousFrame)
          {
            _previousFrame = UnityEngine.Time.frameCount;
            _onClickEvent = new Event();
          }
          return _onClickEvent;
        }
      }
    }
    public static System.Func<float, float, float, float> Clamp = (x, min, max) => x < min ? min : x > max ? max : x;
    public static System.Func<float, float> Clamp01 = x => Clamp(x, 0, 1);
    public static float Map(float x, float inMin, float inMax, float outMin, float outMax, bool clamp = false)
    {
      if (clamp) x = System.Math.Max(inMin, System.Math.Min(x, inMax));
      return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
    public static Rectangle GetCollapseButtonRect(Vec2 pos, float headerHeight, float paddingAmount, float textSize, float width)
    {
      float size = headerHeight * paddingAmount;
      float padding = (headerHeight - size) / 2f;
      float xOffset = textSize * 1.11f;
      var collapseButtonPosition = new Rectangle(pos.x, pos.y, 0, 0);
      collapseButtonPosition.width = xOffset;
      collapseButtonPosition.y += padding;
      collapseButtonPosition.height = size;
      collapseButtonPosition.x = collapseButtonPosition.x + width - xOffset - padding;
      return collapseButtonPosition;
    }
    public static string GetCollapseButtonText(bool state)
    {
      return state ? "[+]" : "[ - ]";
    }
  }
  public static class Aspect
  {
    static float _scale = 1.0f;
    public static float Scale
    {
      get { return _scale; }
      set
      {
        _scale = value;
        OnScaleChange();
      }
    }
    public static float Adjust(float a) { return Scale * a; }
    public static float UnAdjust(float scaled) { return scaled / Scale; }
    public static Vec2 Adjust(Vec2 a)
    {
      return new Vec2(Scale * a.x, Scale * a.y);
    }
    public static Vec2 UnAdjust(Vec2 scaled)
    {
      return new Vec2(scaled.x / Scale, scaled.y / Scale);
    }

    public delegate void OnScaleChangeDelegate();
    public static OnScaleChangeDelegate OnScaleChange = new OnScaleChangeDelegate(() => { });
  }

  public struct Col
  {
    public float r, g, b, a;
    public static Col white = new Col(1, 1, 1, 1);
    public static Col black = new Col(0, 0, 0, 1);
    public static Col red = new Col(1, 0, 0, 1);
    public static Col green = new Col(0, 1, 0, 1);
    public static Col blue = new Col(0, 0, 1, 1);
    public Col(float r = 1, float g = 1, float b = 1, float a = 1)
    {
      this.r = r;
      this.g = g;
      this.b = b;
      this.a = a;
    }
    public static implicit operator Col(UnityEngine.Color rhs)
    {
      return new Col(rhs.r, rhs.g, rhs.b, rhs.a);
    }
    public static implicit operator UnityEngine.Color(Col rhs)
    {
      return new UnityEngine.Color(rhs.r, rhs.g, rhs.b, rhs.a);
    }
  }
  public struct Vec3
  {
    public float x, y, z;
    public Vec3(float x, float y, float z)
    {
      this.x = x; this.y = y; this.z = z;
    }
  }

  public struct Vec2
  {
    public float x, y;
    public Vec2(float x, float y)
    {
      this.x = x;
      this.y = y;
    }

    public static implicit operator UnityEngine.Vector2(Vec2 rhs)
    {
      return new UnityEngine.Vector2(rhs.x, rhs.y);
    }
    public void Normalize()
    {
      var v = new UnityEngine.Vector2(this.x, this.y);
      v.Normalize();
      this.x = v.x;
      this.y = v.y;
    }

    public static Vec2 Perpendicular(Vec2 direction)
    {
      var vec = new UnityEngine.Vector2(direction.x, direction.y);
      return UnityEngine.Vector2.Perpendicular(vec);
    }
    public static Vec2 operator -(Vec2 a, Vec2 b)
    {
      return new Vec2(a.x - b.x, a.y - b.y);
    }
    public static Vec2 operator +(Vec2 a, Vec2 b)
    {
      return new Vec2(a.x + b.x, a.y + b.y);
    }
    public static Vec2 operator *(Vec2 a, float b)
    {
      return new Vec2(a.x * b, a.y * b);
    }
    public static Vec2 operator /(Vec2 a, Vec2 b)
    {
      return new Vec2(a.x / b.x, a.y / b.y);
    }
    public static implicit operator Vec2(UnityEngine.Vector2 rhs)
    {
      return new Vec2(rhs.x, rhs.y);
    }
  }

  public struct Rectangle
  {
    public float x, y, width, height;
    public Rectangle(float x, float y, float w, float h)
    {
      this.x = x; this.y = y; this.width = w; this.height = h;
    }
    public Vec2 Size { get { return new Vec2(width, height); } }
    public Vec2 Position { get { return new Vec2(x, y); } }

    public bool Contains(Vec2 v)
    {
      if (v.x > x && v.x < x + width && v.y > y && v.y < y + height)
      {
        return true;
      }
      return false;
    }
    public override string ToString()
    {
      return string.Format("Rect: [x {0:00}, y {1:00}, w {2:00}, h {3:00}]", this.x, this.y, this.width, this.height);
    }

  }
}