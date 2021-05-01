# GUIPanels

![GUIPanels screenshot](https://i.imgur.com/E7mKxbH.png)

A debug UI for Unity inspired by [ofxUI](https://github.com/rezaali/ofxUI).

## Example

A demo showcasing all widgets.

```C#
using UnityEngine;
using GUIPanels;
public class AllWidgets : MonoBehaviour
{
  Widget _panel1, _panel2, _panel3, _panel4, _panel5;
  public Color rgb;
  public float[] arr = new float[9];
  public bool toggle0;
  public Vector2 range0 = new Vector2(50, 100);
  public string _inputText;
  Theme2 _theme = new Theme2();
  float[] buffer1 = new float[256];
  float[] buffer2 = new float[256];
  public Texture2D _texture;

  public float _dialerValue = 999.999f;

  public bool _toggleButton;

  public Vector2 _samplerPosition;

  public float _scale = 2;
  void Awake()
  {
    Theme.Current = _theme;
    Aspect.Scale = 2;

    #region Panel1
    _panel1 = new WindowPanel("PANEL 1: BASICS").AddToDrawQueue()
      .Attach(
        new Spacer(),
        new Label("Press 'h' to Hide GUIs")
          .Style
            .Font(Theme.Current.FontSmall)
            .Owner,
        new Spacer(),
        new Label("H SLIDERS"),
        new Slider("RED", () => rgb.r * 255, x => rgb.r = x / 255f, 0, 255),
        new Slider("GREEN", () => rgb.g * 255, x => rgb.g = x / 255f, 0, 255),
        new Slider("BLUE", () => rgb.b * 255, x => rgb.b = x / 255f, 0, 255),
        new Spacer(),
        new Label("V SLIDERS"),
        new HorizontalGrid()
          .Attach(
            new VerticalSlider("0", () => arr[0], x => arr[0] = x, 0, 255, 10, 86),
            new VerticalSlider("1", () => arr[1], x => arr[1] = x, 0, 255, 10, 86),
            new VerticalSlider("2", () => arr[2], x => arr[2] = x, 0, 255, 10, 86),
            new VerticalSlider("3", () => arr[3], x => arr[3] = x, 0, 255, 10, 86),
            new VerticalSlider("4", () => arr[4], x => arr[4] = x, 0, 255, 10, 86),
            new VerticalSlider("5", () => arr[5], x => arr[5] = x, 0, 255, 10, 86),
            new VerticalSlider("6", () => arr[6], x => arr[6] = x, 0, 255, 10, 86),
            new VerticalSlider("7", () => arr[7], x => arr[7] = x, 0, 255, 10, 86),
            new VerticalSlider("8", () => arr[8], x => arr[8] = x, 0, 255, 10, 86)
          ),
        new Spacer(),
        new SelectHorizontal(new string[] { "RAD1", "RAD2", "RAD3" }, 0),
        new SelectVertical(new string[] { "RAD1", "RAD2", "RAD3" }, 0),
        new Spacer(),
        new ToggleButton("Button", () => Debug.Log("Pressed")),
        new Toggle("RAD1", () => toggle0, x => toggle0 = x),
        new Spacer(),
        new Label("RANGE SLIDER"),
        new RangeSlider("RSLIDER", () => range0, x => range0 = x, 0, 255),
        new Spacer(),
        new Paragraph("This widget is a text area widget. "
          + "Use this when you need to display a paragraph of text. "
          + "It takes care of formatting the text to fit the block.")
          .Style
            .Font(Theme.Current.FontSmall)
            .Owner
      );
    #endregion

    #region Panel2
    _panel2 = new WindowPanel("PANEL 2: ADVANCED").AddToDrawQueue()
      .Attach(
        new Spacer(),
        new TextField("Text field", () => _inputText, x => _inputText = x),
        new Spacer(),
        new Label("WAVEFORM DISPLAY"),
        new Graph("Waveform", buffer1, 0, 1, 200, 35).SetGraphStyle(GraphStyle.Waveform),
        new Label("SPECTRUM DISPLAY"),
        new Graph("Spectrum", buffer1, 0, 1, 200, 35).SetGraphStyle(GraphStyle.Spectrum),
        new Label("MOVING GRAPH"),
        new Graph("Moving graph", buffer2, 0, 1, 200, 35).SetGraphStyle(GraphStyle.MovingGraph),
        new Spacer(),
        new Label("IMAGE DISPLAY"),
        new ImageWidget((GUIPanels.Unity.UnityTexture)_texture),
        new Spacer(),
        new Label("FPS LABEL"),
        new ValueLabel("FPS", () => string.Format("{0:0.00}", 1 / Time.deltaTime)),
        new Spacer(),
        new Label("NUMBER DIALER"),
        new NumberDialer("DIALER", () => _dialerValue, x => _dialerValue = x, -10000, 10000, 3).SetTooltip("Drag up and down"),
        new Spacer(),
        new Label("LABEL BUTTON"),
        new Button("LABEL BTN"),
        new Spacer(),
        new Label("LABEL TOGGLES"),
        new ButtonToggle("LABEL TGL", () => _toggleButton, x => _toggleButton = x)
          .Style
            .FontColor(Col.red)
            .Owner
      );
    #endregion

    #region Panel3
    _panel3 = new WindowPanel("PANEL 3: ADVANCED").AddToDrawQueue()
      .Attach(
        new Spacer(),
        new Label("MATRIX"),
        new ToggleMatrix("MATRIX1", 3, 3),
        new ToggleMatrix("MATRIX2", 3, 6),
        new ToggleMatrix("MATRIX3", 1, 4),
        new Spacer(),
        new SortableList("SORTABLE LIST", new string[]{
          "FIRST ITEM",
          "SECOND ITEM",
          "THIRD ITEM",
          "FOURTH ITEM",
          "FIFTH ITEM",
          "SIXTH ITEM"
        }),
        new DropdownList(new string[] { "first", "second", "third", "fifth" }, 0)
      );
    #endregion


    #region Panel4
    _panel4 = new WindowPanel("PANEL 4: ADVANCED").AddToDrawQueue()
      .Attach(
        new Spacer(),
        new ImageSampler((GUIPanels.Unity.UnityTexture)_texture, 0, 0, a => rgb = a, () => _samplerPosition, a => _samplerPosition = a)
      );
    #endregion



    #region Panel4
    _panel5 = new WindowPanel("PANEL 5: ADVANCED").AddToDrawQueue()
      .Attach(
        new Spacer()
      );
    #endregion

    _panel2.Position = new Vec2(1 * (_theme.WindowPanelWidth + 2), 0);
    _panel3.Position = new Vec2(2 * (_theme.WindowPanelWidth + 2), 0);
    _panel4.Position = new Vec2(3 * (_theme.WindowPanelWidth + 2), 0);
    _panel5.Position = new Vec2(4 * (_theme.WindowPanelWidth + 2), 0);
  }


  void Update()
  {
    for (int x = 0; x < 256; x++)
    {
      buffer1[x] = Mathf.PerlinNoise(x * 0.02f, Time.time);
      buffer2[x] = Mathf.PerlinNoise(x * 0.1f + Time.time, 0);
    }

    _panel1.AspectScale = _scale;
  }
}
```
