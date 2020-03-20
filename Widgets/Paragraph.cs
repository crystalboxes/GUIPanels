namespace GUIPanels
{
  public class Paragraph : VerticalLayout
  {
    string _text;
    public Paragraph(string text = "", float maxWidth = 100) : base(maxWidth)
    {
      _text = text;
      _defaultWidth = maxWidth;
    }
    public Paragraph(float maxWidth = 100) : base(maxWidth)
    {
      _text = "";
      _defaultWidth = maxWidth;
    }
    public override void SetParent(Widget parent)
    {
      base.SetParent(parent);
      CalculateTextBox();
    }
    public Widget SetText(string text)
    {
      _text = text;
      return this;
    }

    float _defaultWidth;
    public Widget CalculateTextBox()
    {
      const float widthMultiplier = 1f;
      Children.Clear();
      Style.Set<float>(Styles.Width, _defaultWidth);
      float w = InnerWidth * widthMultiplier;
      // split by lines
      var lines = _text.Split('\n');
      string currentLabelText = "";
      foreach (var line in lines)
      {
        // split by word
        var words = line.Split(' ');
        currentLabelText = words[0];
        for (int x = 1; x < words.Length; x++)
        {
          var newText = currentLabelText + " " + words[x];
          if (Utils.CalcSize(newText, Style).x > w)
          {
            Attach(new Label(currentLabelText));
            currentLabelText = words[x];
          }
          else
          {
            currentLabelText = newText;
          }
        }
      }

      if (Children.Count == 0 && currentLabelText != "")
      {
        Attach(new Label(currentLabelText));
        Style.Set<float>(Styles.Width, Utils.CalcSize(currentLabelText, Style).x);
      }
      return this;
    }
  }
}
