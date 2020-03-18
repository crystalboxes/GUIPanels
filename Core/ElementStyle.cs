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
  }
  public enum HorizontalAlignment { Center, Left, Right }
  public class ElementStyleBase
  {
    ////////////////////////
    public ElementStyleBase SetHidden(bool value = true)
    {
      Set(Styles.Hidden, value);
      return this;
    }
    public ElementStyleBase Font(float size)
    {
      return Font(size, Col.black);
    }

    public ElementStyleBase Font(float size, Col color)
    {
      Set(Styles.FontColor, color);
      Set(Styles.FontSize, size);
      return this;
    }

    public Vec2 Position() { return Get<Vec2>(Styles.Position); }
    public ElementStyleBase Position(Vec2 pos)
    {
      Set(Styles.Position, pos);
      return this;
    }

    public Col Background() { return Get<Col>(Styles.BackgroundColor); }
    public ElementStyleBase Background(Col color)
    {
      Set(Styles.BackgroundColor, color);
      return this;
    }
    public Dim Padding() { return Get<Dim>(Styles.Padding); }
    public ElementStyleBase Padding(float dim) { Set(Styles.Padding, new Dim(dim)); return this; }
    public ElementStyleBase Padding(Dim dim) { Set(Styles.Padding, dim); return this; }
    public ElementStyleBase Padding(float top, float leftRight, float bottom) { Set(Styles.Padding, new Dim(top, leftRight, bottom)); return this; }
    public ElementStyleBase Padding(float topBottom, float leftRight) { Set(Styles.Padding, new Dim(topBottom, leftRight)); return this; }

    public Dim Margin() { return Get<Dim>(Styles.Margin); }
    public ElementStyleBase Margin(Dim dim) { Set(Styles.Margin, dim); return this; }
    public ElementStyleBase Margin(float dim) { Set(Styles.Margin, new Dim(dim)); return this; }
    public ElementStyleBase Margin(float top, float leftRight, float bottom) { Set(Styles.Margin, new Dim(top, leftRight, bottom)); return this; }
    public ElementStyleBase Margin(float topBottom, float leftRight) { Set(Styles.Margin, new Dim(topBottom, leftRight)); return this; }

    public Dim Border() { return Get<Dim>(Styles.Border); }
    public Col BorderColor() { return Get<Col>(Styles.BorderColor); }
    public ElementStyleBase Border(Col color, float dim = 1)
    {
      Set(Styles.Border, new Dim(dim));
      Set(Styles.BorderColor, color);
      return this;
    }
    public ElementStyleBase Border(Col color, Dim dim)
    {
      Set(Styles.Border, dim);
      Set(Styles.BorderColor, color);
      return this;
    }

    ////////////////////////
    public ElementStyleBase(Widget owner)
    {
      Owner = owner;
    }
    public Widget Owner { get; set; }
    ElementStyleBase ParentStyle { get { return Owner == null ? null : Owner.Parent == null ? null : Owner.Parent.Style; } }
    Dictionary<string, object> _properties = new Dictionary<string, object>();
    public virtual ElementStyleBase Set<T>(string propName, T value)
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
        UnityEngine.Debug.Log("Warning: wrong type");
      }
      return default(T);
    }
  }
  public class ElementStyle : ElementStyleBase
  {
    public ElementStyle(Widget owner) : base(owner)
    {
    }
    public static implicit operator Style(ElementStyle style)
    {
      return style.FontStyle;
    }

    public override ElementStyleBase Set<T>(string propName, T value)
    {
      base.Set(propName, value);
      if (propName == Styles.FontSize)
      {
        object val = value;
        FontStyle.FontSize = (float)val;
      }

      if (propName == Styles.FontColor)
      {
        object val = value;
        FontStyle.FontColor = (Col)val;
      }
      return this;
    }
    // this is specific to font rendering
    Style _fontStyle;
    Style FontStyle
    {
      get
      {
        if (_fontStyle == null)
        {
          _fontStyle = Utils.MakeStyle();
        }
        return _fontStyle;
      }
    }
  }
}
