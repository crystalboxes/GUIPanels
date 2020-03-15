using UnityEngine;

namespace GUIPanels.Unity
{
  public partial class Renderer
  {
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

    void Vertex3(float x, float y, float z = -10)
    {
      GL.Vertex3(x, y, z);
    }

    public override void DrawTriangle(Rectangle rect, Col color)
    {
      var rectangle = Utils.ToRect(rect);
      RenderMaterial.SetPass(0);
      // Draw lines
      GL.Begin(GL.TRIANGLES);
      GL.Color(color);
      Vertex3(rectangle.x, rectangle.y);
      Vertex3(rectangle.x + rectangle.width, rectangle.y);
      Vertex3(rectangle.x + 0.5f * rectangle.width, 
        rectangle.y + rectangle.height);
      GL.End();
    }

    public override void DrawTriangle(Vec2 a, Vec2 b, Vec2 c, Col color)
    {
      a = Aspect.Adjust(a);
      b = Aspect.Adjust(b);
      c = Aspect.Adjust(c);
      RenderMaterial.SetPass(0);
      // Draw lines
      GL.Begin(GL.TRIANGLES);
      GL.Color(color);
      Vertex3(a.x, a.y);
      Vertex3(b.x, b.y);
      Vertex3(c.x, c.y);
      GL.End();
    }

    public override void DrawLine(Vec2 p0, Vec2 p1, float width, Col color)
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

      RenderMaterial.SetPass(0);
      GL.Begin(GL.TRIANGLES);
      GL.Color(color);

      DrawCircleCommands(p0, halfWidth, color);
      DrawCircleCommands(p1, halfWidth, color);

      Vertex3(start0.x, start0.y);
      Vertex3(end0.x, end0.y);
      Vertex3(end1.x, end1.y);

      Vertex3(start0.x, start0.y);
      Vertex3(end1.x, end1.y);
      Vertex3(start1.x, start1.y);

      GL.End();
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

      RenderMaterial.SetPass(0);
      GL.Begin(GL.TRIANGLES);
      GL.Color(color);

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

      GL.End();
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

      RenderMaterial.SetPass(0);
      GL.Begin(GL.TRIANGLES);
      GL.Color(color);
      DrawCircleCommands(center, radius, color);
      GL.End();
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

      RenderMaterial.SetPass(0);
      GL.Begin(GL.TRIANGLES);
      GL.Color(color);

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
      GL.End();
    }

    public override void DrawRect(Rectangle rectangle1, Col color)
    {
      var rectangle = Utils.ToRect(rectangle1);
      RenderMaterial.SetPass(0);
      // Draw lines
      GL.Begin(GL.TRIANGLES);
      GL.Color(color);
      Vertex3(rectangle.x, rectangle.y);
      Vertex3(rectangle.x + rectangle.width, rectangle.y);
      Vertex3(rectangle.x, rectangle.y + rectangle.height);

      Vertex3(rectangle.x + rectangle.width, rectangle.y);
      Vertex3(rectangle.x + rectangle.width, rectangle.y + rectangle.height);
      Vertex3(rectangle.x, rectangle.y + rectangle.height);
      GL.End();
    }
  }
}
