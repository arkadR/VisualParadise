using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
  public class Menu : MonoBehaviour
  {
    public GameObject mainMenu;
    public GameObject loadMenu;
    public GameObject newGraphMenu;
    public Text playerMovementText;
    private const string c_playerMovement = "Player movement";

    // Start is called before the first frame update
    void Start()
    {
      LoadMainMenu();
      SetPlayerMovementText(PlayerPrefs.GetInt(Constants.PlayerMovementMode));
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
      PlayerMovementMode playerMovementMode = SwitchPlayerMovement();
      SetPlayerMovementText(playerMovementMode);
    }

    private void SetPlayerMovementText(object value)
    {
      playerMovementText.text = $"{c_playerMovement}: {System.Enum.GetName(typeof(PlayerMovementMode), value)}";
    }

    private PlayerMovementMode SwitchPlayerMovement()
    {
      var playerMovementModeValue = PlayerPrefs.GetInt(Constants.PlayerMovementMode);
      var newValue = (playerMovementModeValue + 1) % System.Enum.GetValues(typeof(PlayerMovementMode)).Length;
      PlayerPrefs.SetInt(Constants.PlayerMovementMode, newValue);
      Debug.Log("PlayerMovementMode: " + newValue);
      return (PlayerMovementMode)(newValue);
    }

    public void QuitButton_OnClick()
    {
      Application.Quit();
    }
  }
}
