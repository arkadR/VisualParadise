using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
  /// <summary>
  ///   It calculates frames/second over each updateInterval,
  ///   so the display does not keep changing wildly.
  ///   It is also fairly accurate at very low FPS counts (10).
  ///   We do this not by simply counting frames per interval, but
  ///   by accumulating FPS for each frame. This way we end up with
  ///   correct overall FPS even if the interval renders something like
  ///   5.5 frames.
  /// </summary>
  //http://wiki.unity3d.com/index.php/FramesPerSecond?_ga=2.151047673.1365154948.1591732977-157071781.1591530754
  public class FpsCounter : MonoBehaviour
  {
    float _accum; // FPS accumulated over the interval
    int _frames; // Frames drawn over the interval
    float _timeLeft; // Left time for current interval

    public Text text;
    public float updateInterval = 0.5F;

    void Start()
    {
      if (text == null)
      {
        Debug.Log("FpsCounter needs a Text component!");
        enabled = false;
        return;
      }

      _timeLeft = updateInterval;
    }

    void Update()
    {
      _timeLeft -= Time.deltaTime;
      _accum += Time.timeScale / Time.deltaTime;
      ++_frames;

      // Interval ended - update GUI text and start new interval
      if (!(_timeLeft <= 0.0))
        return;
      var fps = _accum / _frames;
      text.text = $"{fps:F2} FPS"; // display two fractional digits (f2 format)
      if (fps < 30)
        text.color = Color.yellow;
      else if (fps < 10)
        text.color = Color.red;
      else
        text.color = Color.green;
      _timeLeft = updateInterval;
      _accum = 0.0F;
      _frames = 0;
    }
  }
}
