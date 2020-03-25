using System.Collections.Generic;
using UnityEngine;

namespace GUIPanels
{
  public interface IUIRenderer
  {
    void AddWidget(Widget w);
    void PushZ();
    void PopZ();
    List<DrawingState.ShapeData> CurrentShape { get; }
    List<DrawingState.ContentData> CurrentText { get; }
  }
  public class DrawingState
  {
    public struct ShapeData
    {
      public float x, y, z, w;
      public ShapeData(float x, float y, float z, float w)
      {
        this.x = x; this.y = y; this.z = z; this.w = w;
      }
    }
    public struct ContentData
    {
      public Rectangle rectangle;
      public ElementStyle style;
      public string text;
      public Texture texture;
    }
    public List<ShapeData> Shape = new List<ShapeData>();
    public List<ContentData> Text = new List<ContentData>();
  }

  public class WidgetBatch
  {
    public Widget Widget;
    public List<DrawingState> DrawingStates;
    public int Current = 0;

  }
  public class UIRenderer : UnityEngine.MonoBehaviour, IUIRenderer
  {
    List<WidgetBatch> _batches = new List<WidgetBatch>();
    WidgetBatch _current;
    public void AddWidget(Widget w)
    {
      var states = new List<DrawingState>();
      states.Add(new DrawingState());
      _batches.Add(new WidgetBatch()
      {
        Widget = w,
        DrawingStates = states
      });
    }
    public List<DrawingState.ShapeData> CurrentShape
    {
      get
      {
        return _current.DrawingStates[_current.Current].Shape;
      }
    }

    public List<DrawingState.ContentData> CurrentText
    {
      get
      {
        return _current.DrawingStates[_current.Current].Text;
      }
    }
    public void PushZ()
    {
      _current.Current++;
      if (_current.Current == _current.DrawingStates.Count)
      {
        _current.DrawingStates.Add(new DrawingState());
      }
    }

    public void PopZ()
    {
      _current.Current--;
      if (_current.Current < 0)
      {
        throw new System.Exception("Couldn't have negative index");
      }
    }

    void OnGUI()
    {
      foreach (var batch in _batches)
      {

        var scale = Aspect.Scale;
        Aspect.Scale = batch.Widget.AspectScale;

        _current = batch;
        _current.Current = 0;
        if (batch.Widget.CurrentStyle.Get<bool>(Styles.Hidden))
        {
          continue;
        }
        foreach (var _state in batch.DrawingStates)
        {
          _state.Shape.Clear();
          _state.Text.Clear();
        }
        _current.Widget.Draw();
        Aspect.Scale = scale;
      }

      // draw each batch
      foreach (var batch in _batches)
      {
        var scale = Aspect.Scale;
        Aspect.Scale = batch.Widget.AspectScale;
        foreach (var state in batch.DrawingStates)
        {
          (Renderer.Current as Unity.Renderer).SetPass2(0);
          GL.Begin(GL.TRIANGLES);
          foreach (var s in state.Shape)
          {
            if (s.w < 0)
            {
              GL.Vertex3(s.x, s.y, s.z);
            }
            else
            {
              GL.Color(new Color(s.x, s.y, s.z, s.w));
            }
          }
          GL.End();

          foreach (var t in state.Text)
          {
            if (t.texture != null)
            {
              GUI.DrawTexture(Utils.ToRect(t.rectangle), t.texture as Unity.UnityTexture);
            }

            if (t.style != null)
            {
              Utils.CurrentStyle.FontSize = t.style.Get<float>(Styles.FontSize);
              Utils.CurrentStyle.FontColor = t.style.Get<Col>(Styles.FontColor);
              GUI.Label(Utils.ToRect(t.rectangle), t.text, Utils.CurrentStyle.GUIStyle);
            }
          }
        }
        Aspect.Scale = scale;
      }
      _current = null;
    }
  }
}
