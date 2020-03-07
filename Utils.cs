namespace GUIPanels
{
  public static class Aspect
  {
    public static float Scale = 1.0f;
    public static float Adjust(float a) { return Scale * a; }
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
  }
  public struct Vec2
  {
    public float x, y;
    public Vec2(float x, float y)
    {
      this.x = x;
      this.y = y;
    }
    public static Vec2 operator -(Vec2 a, Vec2 b)
    {
      return new Vec2(a.x - b.x, a.y - b.y);
    }
    public static Vec2 operator +(Vec2 a, Vec2 b)
    {
      return new Vec2(a.x + b.x, a.y + b.y);
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


    public bool Contains(Vec2 v)
    {
      if (v.x > x && v.x < x + width && v.y > y && v.y < y + height)
      {
        return true;
      }
      return false;
    }
  }
}