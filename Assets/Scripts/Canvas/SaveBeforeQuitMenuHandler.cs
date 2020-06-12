using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Canvas
{
  class SaveBeforeQuitMenuHandler : MonoBehaviour
  {
    public void QuitButton_OnClick() => SceneManager.LoadScene(Constants.MainMenuScene);
  }
}
