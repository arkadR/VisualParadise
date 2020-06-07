using UnityEngine;

public class GameService : MonoBehaviour
{
  public static GameService Instance;

  void Awake()
  {
    if (Instance != null)
      GameObject.Destroy(Instance);
    else
      Instance = this;

    DontDestroyOnLoad(this);
  }

  public bool IsPaused { get; private set; } = false;

  public void PauseGame() => IsPaused = true;

  public void UnPauseGame() => IsPaused = false;


}
