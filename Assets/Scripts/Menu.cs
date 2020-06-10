using Assets.Scripts.Common.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
  public class Menu : MonoBehaviour
  {
    private const string c_playerMovement = "Player movement";
    public GameObject loadMenu;
    public GameObject mainMenu;
    public GameObject newGraphMenu;
    public Text playerMovementText;

    private int PlayerMovementModeValue
    {
      get => PlayerPrefs.GetInt(Constants.PlayerMovementMode);
      set => PlayerPrefs.SetInt(Constants.PlayerMovementMode, value);
    }

    // Start is called before the first frame update
    public void Start()
    {
      LoadMainMenu();
      SetPlayerMovementText(EnumUtils<PlayerMovementMode>.DefinedOrDefaultCast(PlayerMovementModeValue));
    }

    public void LoadMainMenu()
    {
      mainMenu.SetActive(true);
      loadMenu.SetActive(false);
      newGraphMenu.SetActive(false);
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

    public void PlayerMovement_OnClick()
    {
      PlayerMovementModeValue = EnumUtils<PlayerMovementMode>.GetNextValue(PlayerMovementModeValue);
      Debug.Log("PlayerMovementMode: " + PlayerMovementModeValue);
      SetPlayerMovementText(EnumUtils<PlayerMovementMode>.DefinedOrDefaultCast(PlayerMovementModeValue));
    }

    private void SetPlayerMovementText(PlayerMovementMode value) => playerMovementText.text =
      $"{c_playerMovement}: {EnumUtils<PlayerMovementMode>.GetName(value)}";

    public void QuitButton_OnClick() => Application.Quit();
  }
}
