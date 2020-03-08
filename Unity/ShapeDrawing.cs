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


      GL.Vertex3(start0.x, start0.y, 0);
      GL.Vertex3(end0.x, end0.y, 0);
      GL.Vertex3(end1.x, end1.y, 0);

      GL.Vertex3(start0.x, start0.y, 0);
      GL.Vertex3(end1.x, end1.y, 0);
      GL.Vertex3(start1.x, start1.y, 0);

      GL.End();
    }
    public override void DrawArc(Vec2 center, float radius, float startAngle, float endAngle, float width, Col color)
    {

    }

    void DrawCircleCommands(Vec2 center, float radius, Col color)
    {
      float angle = 0f;
      const float twopi = 2f * (float)System.Math.PI;
      float incr = twopi / (float)CircleResolution;
      while (angle < twopi)
      {
        GL.Vertex3(center.x, center.y, 0);
        GL.Vertex3(
          center.x + (float)System.Math.Sin(angle) * radius,
          center.y + (float)System.Math.Cos(angle) * radius, 0);
        GL.Vertex3(
          center.x + (float)System.Math.Sin(angle + incr) * radius,
          center.y + (float)System.Math.Cos(angle + incr) * radius, 0);
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

    }
    public override void DrawArcRing(Vec2 center, float radius, float innerRadius, float startAngle, float endAngle, Col color)
    {

    }

    public override void DrawRect(Rectangle rectangle1, Col color)
    {
      var rectangle = Utils.ToRect(rectangle1);
      RenderMaterial.SetPass(0);
      // Draw lines
      GL.Begin(GL.TRIANGLES);
      //
      GL.Color(color);
      GL.Vertex3(rectangle.x, rectangle.y, 0);
      GL.Vertex3(rectangle.x + rectangle.width, rectangle.y, 0);
      GL.Vertex3(rectangle.x, rectangle.y + rectangle.height, 0);

      GL.Vertex3(rectangle.x + rectangle.width, rectangle.y, 0);
      GL.Vertex3(rectangle.x + rectangle.width, rectangle.y + rectangle.height, 0);
      GL.Vertex3(rectangle.x, rectangle.y + rectangle.height, 0);
      GL.End();
    }
  }
}
