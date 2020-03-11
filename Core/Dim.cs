namespace GUIPanels
{
  public struct Dim
  {
    public float Top, Right, Bottom, Left;
    public static Dim Zero { get { return new Dim(0); } }

    public static Dim top
    {
      get
      {
        return new Dim()
        {
          Bottom = 0,
          Left = 0,
          Right = 0,
          Top = 1
        };
      }
    }
    public static Dim right
    {
      get
      {
        return new Dim()
        {
          Bottom = 0,
          Left = 0,
          Right = 1,
          Top = 0
        };
      }
    }
    public static Dim left
    {
      get
      {
        return new Dim()
        {
          Bottom = 0,
          Left = 1,
          Right = 0,
          Top = 0
        };
      }
    }

    public static Dim bottom
    {
      get
      {
        return new Dim()
        {
          Bottom = 1,
          Left = 0,
          Right = 0,
          Top = 0
        };
      }
    }

    public static Dim operator +(Dim lhs, Dim rhs)
    {
      return new Dim()
      {
        Bottom = lhs.Bottom + rhs.Bottom,
        Left = lhs.Left + rhs.Left,
        Right = lhs.Right + rhs.Right,
        Top = lhs.Top + rhs.Top
      };
    }
    public static Dim operator *(Dim lhs, float rhs)
    {
      return new Dim()
      {
        Bottom = lhs.Bottom * rhs,
        Left = lhs.Left * rhs,
        Right = lhs.Right * rhs,
        Top = lhs.Top * rhs
      };
    }

    public Dim(float dim)
    {
      Top = Right = Bottom = Left = dim;
    }
    public Dim(float top, float leftRight, float bottom)
    {
      Top = top; Left = leftRight; Right = leftRight; Bottom = bottom;
    }

    public Dim(float topBottom, float leftRight)
    {
      Top = Bottom = topBottom;
      Left = Right = leftRight;
    }
  }
}