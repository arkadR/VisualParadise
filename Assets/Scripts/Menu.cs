using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
  public GameObject mainMenu;

  public GameObject loadMenu;
  // Start is called before the first frame update
  void Start()
  {
    LoadMainMenu();
  }

  public void LoadMainMenu()
  {
    mainMenu.SetActive(true);
    loadMenu.SetActive(false);
  }

  public void LoadButton_OnClick()
  {
    mainMenu.SetActive(false);
    loadMenu.SetActive(true);
  }

  public void QuitButton_OnClick()
  {
    Application.Quit();
  }
}
