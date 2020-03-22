using System;
using System.Collections.Generic;

namespace GUIPanels
{
  public abstract class Theme
  {
    public virtual FontStyle FontSmall
    {
      get
      {
        return new FontStyle()
        {
          Size = FontSizeSmall,
          Color = FontColor
        };
      }
    }
    public virtual FontStyle FontMedium
    {
      get
      {
        return new FontStyle()
        {
          Size = FontSizeMedium,
          Color = FontColor
        };
      }
    }
    public virtual FontStyle FontLarge
    {
      get
      {
        return new FontStyle()
        {
          Size = FontSizeLarge,
          Color = FontColor
        };
      }
    }

    public abstract Col PrimaryColor { get; }
    public abstract Col SecondaryColor { get; }

    public abstract float FontSizeLarge { get; }
    public abstract float FontSizeMedium { get; }
    public abstract float FontSizeSmall { get; }
    public abstract Col FontColor { get; }
    static Theme _current;
    public static Theme Current
    {
      get
      {
        if (_current == null)
        {
          _current = new DefaultTheme();
        }
        return _current;
      }
      set { _current = value; }
    }

    public void Apply(Widget c)
    {
      Apply(c, c.GetType());
    }

    public void Apply(Widget c, System.Type widgetType)
    {
      if (_styles.ContainsKey(widgetType))
      {
        _styles[widgetType](c);
      }
    }

    protected System.Action<Widget> GetAction<T>()
    {
      if (_styles.ContainsKey(typeof(T)))
      {
        return _styles[typeof(T)];
      }
      return null;
    }

    protected System.Action<Widget> GetAction(System.Type widgetType)
    {
      if (_styles.ContainsKey(widgetType))
      {
        return _styles[widgetType];
      }
      return null;
    }

    public Theme()
    {
    }

    protected void Add(System.Action<Widget> style, System.Type widgetType)
    {
      _styles[widgetType] = style;
    }

    protected void Add<T>(System.Action<Widget> style)
    {
      _styles[typeof(T)] = style;
    }

    Dictionary<System.Type, System.Action<Widget>> _styles =
      new Dictionary<System.Type, System.Action<Widget>>();
  }
}
