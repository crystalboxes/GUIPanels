using System;

namespace GUIPanels
{
  public class ToggleButton : Label
  {
    public bool On { get { return _isOn; } }
    Action<bool> _onToggleClicked = null;
    bool _isOn;
    public ToggleButton(bool isOn, Action<bool> onToggleClicked = null) : base("[+]", null, null)
    {
      Style.Set(Styles.Width, 12f);
      _onToggleClicked = onToggleClicked;
      _isOn = isOn;
      UpdateTitle();
    }

    protected override void OnClick()
    {
      base.OnClick();
      // UnityEngine.Debug.Log("hey");
      _isOn = !_isOn;
      if (_onToggleClicked != null)
      {
        _onToggleClicked(_isOn);
      }
      UpdateTitle();
    }
    void UpdateTitle()
    {
      Title = _isOn ? "[-]" : "[+]";
    }

  }
}
