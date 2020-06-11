using Assets.Scripts.Common.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
  public class Menu : MonoBehaviour
  {
    public GameObject loadMenu;
    public GameObject mainMenu;
    public GameObject newGraphMenu;
    public GameObject settingsMenu;

    // Start is called before the first frame update
    public void Start()
    {
      LoadMainMenu();
    }

    public void LoadMainMenu()
    {
      mainMenu.SetActive(true);
      loadMenu.SetActive(false);
      newGraphMenu.SetActive(false);
      settingsMenu.SetActive(false);
    }

    public void NewGraphButton_OnClick()
    {
      newGraphMenu.SetActive(true);
      mainMenu.SetActive(false);
    }

    public void LoadButton_OnClick()
    {
      mainMenu.SetActive(false);
      loadMenu.SetActive(true);
    }

    public void SettingsButton_OnClick()
    {
      mainMenu.SetActive(false);
      settingsMenu.SetActive(true);
    }

    public void QuitButton_OnClick() => Application.Quit();
  }
}
