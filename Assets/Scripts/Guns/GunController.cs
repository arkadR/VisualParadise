using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Guns
{
  public class GunController : MonoBehaviour
  {
    private IDictionary<KeyCode, IGun> _guns;
    private Camera _attachedCamera;
    private GraphService _graphService;
    private ToolGunController _toolGunController;

    private IGun _activeGun;

    public IGun ActiveGun
    {
      get => _activeGun;
      set
      {
        _activeGun = value;
        _toolGunController.SetGunModeText(_activeGun.GunName);
      }
    }

    private void Start()
    {
      _attachedCamera = Camera.main;
      _graphService = FindObjectOfType<GraphService>();
      _toolGunController = FindObjectOfType<ToolGunController>();
      var edgeMaterial = Resources.Load<Material>("Materials/Edge Material");

      _guns = new Dictionary<KeyCode, IGun>
      {
        [KeyCode.Alpha1] = gameObject.AddComponent<NodeGun>(),
        [KeyCode.Alpha2] = new EdgeGun(_graphService, edgeMaterial),
        [KeyCode.Alpha3] = new MovementExecutorGun(FindObjectOfType<MovementExecutor>()),
        [KeyCode.Alpha4] = new GraphArrangerGun(FindObjectOfType<GraphArranger>())
      };

      ActiveGun = _guns.First().Value;
    }

    private void Update()
    {
      if (GameService.Instance.IsPaused)
        return;

      HandleChangeGun();
      HandleFire();
    }

    private void HandleFire()
    {
      if (Input.GetButtonDown("Fire1"))
      {
        ActiveGun.OnMoveDown(transform, _attachedCamera);
      }
      else if (Input.GetButtonDown("Fire2"))
      {
        ActiveGun.OnRightClick(_attachedCamera);
      }
    }

    private void DisableMovementGuns(IGun gunNotToDisable)
    {
      foreach (var gun in guns.Values)
      {
        if (gun != gunNotToDisable && gun is IMovementGun)
          (gun as IMovementGun).Disable();
      }
    }

    private void HandleChangeGun()
    {
      var pressedGunKey = _guns
          .Keys
          .FirstOrDefault(Input.GetKeyDown);

      if (pressedGunKey == KeyCode.None) 
        return;

      var newGun = guns[pressedGunKey];
      if (activeGun == newGun)
        return;

      ActiveGun.OnSwitchedAway();

      if (newGun is IMovementGun)
        DisableMovementGuns(newGun);
      ActiveGun = newGun;
    }
  }
}
