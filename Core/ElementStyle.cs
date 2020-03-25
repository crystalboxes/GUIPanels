using System.Collections.Generic;
namespace GUIPanels
{
  public static class Styles
  {
    public const string Margin = "margin";
    public const string Border = "border";
    public const string Hidden = "hidden";
    public static string Padding { get { return "padding"; } }
    public static string BorderColor { get { return "border-color"; } }
    public static string BackgroundColor { get { return "background-color"; } }
    public static string FontColor { get { return "font-color"; } }
    public static string FontSize { get { return "font-size"; } }
    public static string LineHeight { get { return "line-height"; } }
    public static string TextAlign { get { return "text-align"; } }
    public static string Position { get { return "position"; } }
    public static string Width { get { return "width"; } }
    public static string Height { get { return "height"; } }
    public static string Outline { get { return "outline"; } }
  }
  public enum HorizontalAlignment { Center, Left, Right }
  public struct FontStyle
  {
    public Col Color;
    public float Size;
  }
  public struct Outline
  {
    public Col Color;
    public float Width;
  }

  public class ElementStyle
  {
    public ElementStyle SetHidden(bool value = true)
    {
      Set(Styles.Hidden, value);
      return this;
    }
    public Col FontColor()
    {
      return Get<Col>(Styles.FontColor);
    }
    public ElementStyle FontColor(Col color)
    {
      Set(Styles.FontColor, color);
      return this;
    }

    public float FontSize()
    {
      return Get<float>(Styles.FontSize);
    }
    public ElementStyle FontSize(float size)
    {
      Set(Styles.FontSize, size);
      return this;
    }

    public ElementStyle Font(FontStyle style)
    {
      Set(Styles.FontColor, style.Color);
      Set(Styles.FontSize, style.Size);
      return this;
    }

    public ElementStyle Font(float size)
    {
      Set(Styles.FontSize, size);
      return this;
    }

    public ElementStyle Font(float size, Col color)
    {
      Set(Styles.FontColor, color);
      Set(Styles.FontSize, size);
      return this;
    }

    public Vec2 Position() { return Get<Vec2>(Styles.Position); }
    public ElementStyle Position(Vec2 pos)
    {
      Set(Styles.Position, pos);
      return this;
    }
    public ElementStyle Position(float x, float y)
    {
      Set(Styles.Position, new Vec2(x, y));
      return this;
    }

    public Col Background() { return Get<Col>(Styles.BackgroundColor); }
    public ElementStyle Background(Col color)
    {
      Set(Styles.BackgroundColor, color);
      return this;
    }

    public ElementStyle Outline(float width, Col color)
    {
      Set<Outline>(Styles.Outline, new GUIPanels.Outline() { Width = width, Color = color });
      return this;
    }
    public GUIPanels.Outline Outline()
    {
      return Get<GUIPanels.Outline>(Styles.Outline);
    }

    public Dim Padding() { return Get<Dim>(Styles.Padding); }
    public ElementStyle Padding(float dim) { Set(Styles.Padding, new Dim(dim)); return this; }
    public ElementStyle Padding(Dim dim) { Set(Styles.Padding, dim); return this; }
    public ElementStyle Padding(float top, float leftRight, float bottom) { Set(Styles.Padding, new Dim(top, leftRight, bottom)); return this; }
    public ElementStyle Padding(float topBottom, float leftRight) { Set(Styles.Padding, new Dim(topBottom, leftRight)); return this; }

    public Dim Margin() { return Get<Dim>(Styles.Margin); }
    public ElementStyle Margin(Dim dim) { Set(Styles.Margin, dim); return this; }
    public ElementStyle Margin(float dim) { Set(Styles.Margin, new Dim(dim)); return this; }
    public ElementStyle Margin(float top, float leftRight, float bottom) { Set(Styles.Margin, new Dim(top, leftRight, bottom)); return this; }
    public ElementStyle Margin(float topBottom, float leftRight) { Set(Styles.Margin, new Dim(topBottom, leftRight)); return this; }

    public Dim Border() { return Get<Dim>(Styles.Border); }
    public Col BorderColor() { return Get<Col>(Styles.BorderColor); }
    public ElementStyle Border(float dim, Col color)
    {
      Set(Styles.Border, new Dim(dim));
      Set(Styles.BorderColor, color);
      return this;
    }
    public ElementStyle Border(Col color, float dim = 1)
    {
      Set(Styles.Border, new Dim(dim));
      Set(Styles.BorderColor, color);
      return this;
    }
    public ElementStyle Border(Col color, Dim dim)
    {
      Set(Styles.Border, dim);
      Set(Styles.BorderColor, color);
      return this;
    }

    ////////////////////////
    ElementStyle _baseStyle;
    ElementStyle _clicked, _hovered;
    public ElementStyle Clicked
    {
      get
      {
        if (_baseStyle != null)
        {
          return null;
        }
        if (_clicked == null)
        {
          _clicked = new ElementStyle(Owner);
          _clicked._baseStyle = this;
        }
        return _clicked;
      }
    }

    public ElementStyle Hovered
    {
      get
      {
        if (_baseStyle != null)
        {
          return null;
        }
        if (_hovered == null)
        {
          _hovered = new ElementStyle(Owner);
          _hovered._baseStyle = this;
        }
        return _hovered;
      }
    }

    /////////////
    public ElementStyle(Widget owner)
    {
      Owner = owner;
    }
    public Widget Owner { get; set; }
    protected ElementStyle ParentStyle
    {
      get
      {
        if (_baseStyle == null)
        {
          return Owner == null ? null : Owner.Parent == null ? null : Owner.Parent.CurrentStyle;
        }

        return _baseStyle;
      }
    }
    Dictionary<string, object> _properties = new Dictionary<string, object>();
    public virtual ElementStyle Set<T>(string propName, T value)
    {
      _properties[propName] = value;
      return this;
    }

    public void RemoveProperty(string propName)
    {
      _properties.Remove(propName);
    }
    public T Get<T>(string propName)
    {
      if (!_properties.ContainsKey(propName))
      {
        if (_baseStyle != null)
        {
          return _baseStyle.Get<T>(propName);
        }
        if (ParentStyle == null)
        {
          return default(T);
        }
        return ParentStyle.Get<T>(propName);
      }
      // unbox value
      try
      {
        return (T)_properties[propName];
      }
      catch (System.InvalidCastException e)
      {
        UnityEngine.Debug.LogFormat("Warning: wrong type {0}", e);
      }
      return default(T);
    }
    public bool Exists(string propName)
    {
      return _properties.ContainsKey(propName);
    }
  }
}
