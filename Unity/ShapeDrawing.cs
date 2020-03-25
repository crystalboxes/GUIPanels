using UnityEngine;

namespace GUIPanels.Unity
{
  public partial class Renderer
  {
    bool _immediateMode = false;
    public override bool ImmediateMode
    {
      get { return _immediateMode; }
      set { _immediateMode = value; }
    }

    bool _setPassInvalidated = true;

    public void SetPass2(int _ = 0)
    {
      RenderMaterial.SetPass(_);
    }

     void SetPass(int _ = 0)
    {
      if (!ImmediateMode) { return; }
      if (_setPassInvalidated)
      {
        _setPassInvalidated = false;
        RenderMaterial.SetPass(_);
      }
    }

    void GLBegin(int state)
    {
      if (!ImmediateMode) { return; }
      GL.Begin(state);
    }

    void GLEnd()
    {
      if (!ImmediateMode) { return; }
      GL.End();
    }

    void GLColor(Color color)
    {
      if (!ImmediateMode)
      {
        Rendering.UIRenderer.CurrentShape.Add(
          new DrawingState.ShapeData(color.r, color.g, color.b, color.a));
        return;
      }
      GL.Color(color);
    }

    void Vertex3(float x, float y, float z = -10)
    {
      if (!ImmediateMode)
      {
        Rendering.UIRenderer.CurrentShape.Add(
          new DrawingState.ShapeData(x, y, z, -1));
        return;
      }
      GL.Vertex3(x, y, z);
    }

    public override void DrawTexture(Rectangle rectangle, Texture texture)
    {
      _setPassInvalidated = true;
      var t = (texture as UnityTexture).Tex;
      if (t == null)
      {
        return;
      }

      if (!ImmediateMode)
      {
        Rendering.UIRenderer.CurrentText.Add(new DrawingState.ContentData()
        {
          rectangle = rectangle,
          texture = texture
        });
        return;
      }

      GUI.DrawTexture(Utils.ToRect(rectangle), t);
    }

    public override void DrawText(Rectangle rectangle, string text, ElementStyle style)
    {
      _setPassInvalidated = true;
      Utils.CurrentStyle.FontSize = style.Get<float>(Styles.FontSize);
      Utils.CurrentStyle.FontColor = style.Get<Col>(Styles.FontColor);

      if (!ImmediateMode)
      {
        Rendering.UIRenderer.CurrentText.Add(new DrawingState.ContentData()
        {
          rectangle = rectangle,
          text = text,
          style = style
        });
        return;
      }
      GUI.Label(Utils.ToRect(rectangle), text, Utils.CurrentStyle.GUIStyle);
    }


    Material _material;
    Material RenderMaterial
    {
      get
      {
        if (!_material)
        {
          // Unity has a built-in shader that is useful for drawing
          // simple colored things.
          Shader shader = Shader.Find("Hidden/Internal-Colored");
          var lineMaterial = new Material(shader);
          lineMaterial.hideFlags = HideFlags.HideAndDontSave;
          // Turn on alpha blending
          lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
          lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
          // Turn backface culling off
          lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
          // Turn off depth writes
          lineMaterial.SetInt("_ZWrite", 0);
          _material = lineMaterial;
        }
        return _material;
      }
    }


    public override void BeginTriangleShape(Col color)
    {
      SetPass(0);
      // Draw lines
      GLBegin(GL.TRIANGLES);
      GLColor(color);
    }
    public override void EndShape()
    {
      GLEnd();
    }
    public override void AddVertex(float x, float y, float z)
    {
      Vertex3(x, y, z);
    }


    public override void DrawTriangle(Rectangle rect, Col color)
    {
      var rectangle = Utils.ToRect(rect);
      SetPass(0);
      // Draw lines
      GLBegin(GL.TRIANGLES);
      GLColor(color);
      Vertex3(rectangle.x, rectangle.y);
      Vertex3(rectangle.x + rectangle.width, rectangle.y);
      Vertex3(rectangle.x + 0.5f * rectangle.width,
        rectangle.y + rectangle.height);
      GLEnd();
    }

    public override void DrawTriangle(Vec2 a, Vec2 b, Vec2 c, Col color)
    {
      a = Aspect.Adjust(a);
      b = Aspect.Adjust(b);
      c = Aspect.Adjust(c);
      SetPass(0);
      // Draw lines
      GLBegin(GL.TRIANGLES);
      GLColor(color);
      Vertex3(a.x, a.y);
      Vertex3(b.x, b.y);
      Vertex3(c.x, c.y);
      GLEnd();
    }

    public override void DrawLineBox(Rectangle rect, float width, Col color)
    {
      var pos = rect.Position;
      var dim = rect.Size;
      var a = pos; var b = a + new Vec2(dim.x, 0);
      var c = b + new Vec2(0, dim.y); var d = a + new Vec2(0, dim.y);
      Rendering.DrawLine(a + new Vec2(width * 0.5f, 0), b - new Vec2(width * 0.5f, 0), width, color);
      Rendering.DrawLine(b, c, width, color);
      Rendering.DrawLine(c - new Vec2(width * 0.5f, 0), d + new Vec2(width * 0.5f, 0), width, color);
      Rendering.DrawLine(d, a, width, color);
    }

    public override void DrawLine(Vec2 p0, Vec2 p1, float width, Col color, bool isRounded)
    {
      p0 = Aspect.Adjust(p0);
      p1 = Aspect.Adjust(p1);
      width = Aspect.Adjust(width);

      var dir = p1 - p0;
      dir.Normalize();
      var perpendicular = Vec2.Perpendicular(dir);
      perpendicular.Normalize();

      float halfWidth = width * 0.5f;
      var start0 = p0 + perpendicular * halfWidth;
      var start1 = p0 - perpendicular * halfWidth;

      var end0 = p1 + perpendicular * halfWidth;
      var end1 = p1 - perpendicular * halfWidth;

      // draw circles
      var res = CircleResolution;
      CircleResolution = 8;
      CircleResolution = res;

      SetPass(0);
      GLBegin(GL.TRIANGLES);
      GLColor(color);

      if (isRounded)
      {
        DrawCircleCommands(p0, halfWidth, color);
        DrawCircleCommands(p1, halfWidth, color);
      }

      Vertex3(start0.x, start0.y);
      Vertex3(end0.x, end0.y);
      Vertex3(end1.x, end1.y);

      Vertex3(start0.x, start0.y);
      Vertex3(end1.x, end1.y);
      Vertex3(start1.x, start1.y);

      GLEnd();
    }



    public override void DrawArc(Vec2 center, float radius, float startAngle, float endAngle, Col color)
    {
      if (endAngle < startAngle)
      {
        var c = startAngle;
        startAngle = endAngle;
        endAngle = startAngle;
      }

      center = Aspect.Adjust(center);
      radius = Aspect.Adjust(radius);

      startAngle *= DEG2RAD;
      float angle = DEG2RAD;
      endAngle = endAngle * DEG2RAD;
      float incr = (endAngle - angle) / (float)CircleResolution;

      SetPass(0);
      GLBegin(GL.TRIANGLES);
      GLColor(color);

      while (angle < endAngle)
      {
        Vertex3(center.x, center.y);
        Vertex3(
          center.x + (float)System.Math.Sin(Utils.Clamp(angle, startAngle, endAngle)) * radius,
          center.y + (float)System.Math.Cos(Utils.Clamp(angle, startAngle, endAngle)) * radius);
        Vertex3(
          center.x + (float)System.Math.Sin(Utils.Clamp(angle + incr, startAngle, endAngle)) * radius,
          center.y + (float)System.Math.Cos(Utils.Clamp(angle + incr, startAngle, endAngle)) * radius);
        angle += incr;
      }

      GLEnd();
    }

    void DrawCircleCommands(Vec2 center, float radius, Col color)
    {
      float angle = 0f;
      const float twopi = 2f * (float)System.Math.PI;
      float incr = twopi / (float)CircleResolution;
      while (angle < twopi)
      {
        Vertex3(center.x, center.y);
        Vertex3(
          center.x + (float)System.Math.Sin(angle) * radius,
          center.y + (float)System.Math.Cos(angle) * radius);
        Vertex3(
          center.x + (float)System.Math.Sin(angle + incr) * radius,
          center.y + (float)System.Math.Cos(angle + incr) * radius);
        angle += incr;
      }
    }

    public override void DrawCircle(Vec2 center, float radius, Col color)
    {
      center = Aspect.Adjust(center);
      radius = Aspect.Adjust(radius);

      SetPass(0);
      GLBegin(GL.TRIANGLES);
      GLColor(color);
      DrawCircleCommands(center, radius, color);
      GLEnd();
    }

    public override void DrawRing(Vec2 center, float radius, float innerRadius, Col color)
    {
      DrawArcRing(center, radius, innerRadius, 0, 360, color);
    }

    const float DEG2RAD = 0.0174533f;
    public override void DrawArcRing(Vec2 center, float radius, float innerRadius, float startAngle, float endAngle, Col color)
    {
      var ct = Aspect.Adjust(center);
      var r = Aspect.Adjust(radius);
      var ri = Aspect.Adjust(innerRadius);

      if (endAngle < startAngle)
      {
        var c = startAngle;
        startAngle = endAngle;
        endAngle = startAngle;
      }

      if (ri < float.Epsilon)
      {
        DrawArc(center, radius, startAngle, endAngle, color);
        return;
      }

      // Make sure that inner radius is smaller than the main radius
      if (ri > r)
      {
        ri = r - (ri - r);
      }

      SetPass(0);
      GLBegin(GL.TRIANGLES);
      GLColor(color);

      startAngle *= DEG2RAD;
      float angle = startAngle;
      endAngle = endAngle * DEG2RAD;

      float incr = (endAngle - angle) / (float)CircleResolution;
      while (angle < endAngle)
      {
        float s = (float)System.Math.Sin(Utils.Clamp(angle, startAngle, endAngle));
        float c = (float)System.Math.Cos(Utils.Clamp(angle, startAngle, endAngle));
        float si = (float)System.Math.Sin(Utils.Clamp(angle + incr, startAngle, endAngle));
        float ci = (float)System.Math.Cos(Utils.Clamp(angle + incr, startAngle, endAngle));

        Vertex3(ct.x + s * r, ct.y + c * r);
        Vertex3(ct.x + si * r, ct.y + ci * r);
        Vertex3(ct.x + s * ri, ct.y + c * ri);

        Vertex3(ct.x + s * ri, ct.y + c * ri);
        Vertex3(ct.x + si * r, ct.y + ci * r);
        Vertex3(ct.x + si * ri, ct.y + ci * ri);

        angle += incr;
      }
      GLEnd();
    }

    public override void DrawRect(Rectangle rectangle1, Col color)
    {
      var rectangle = Utils.ToRect(rectangle1);
      SetPass(0);
      // Draw lines
      GLBegin(GL.TRIANGLES);
      GLColor(color);
      Vertex3(rectangle.x, rectangle.y);
      Vertex3(rectangle.x + rectangle.width, rectangle.y);
      Vertex3(rectangle.x, rectangle.y + rectangle.height);

      Vertex3(rectangle.x + rectangle.width, rectangle.y);
      Vertex3(rectangle.x + rectangle.width, rectangle.y + rectangle.height);
      Vertex3(rectangle.x, rectangle.y + rectangle.height);
      GLEnd();
    }
  }
}
