using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Canvas;
using UnityEngine;

namespace Assets.Scripts.Guns
{
  public class GunController : MonoBehaviour
  {
    private GunTypeChange gunTypeChange;
    private IDictionary<KeyCode, IGun> guns;
    private Camera attachedCamera;
    private GraphService graphService;
    private GraphLoader graphLoader;
    private IGun _activeGun;
    private IGun activeGun
    {
      get => _activeGun;
      set
      {
        gunTypeChange.ChangeGunType(value.GunName);
        _activeGun = value;
      }
    }

    private void Start()
    {
      attachedCamera = Camera.main;
      graphService = FindObjectOfType<GraphService>();
      gunTypeChange = FindObjectOfType<GunTypeChange>();
      var edgeMaterial = Resources.Load<Material>("Materials/Edge Material");

      guns = new Dictionary<KeyCode, IGun>
      {
        [KeyCode.Alpha1] = gameObject.AddComponent<NodeGun>(),
        [KeyCode.Alpha2] = new EdgeGun(graphService, edgeMaterial),
        [KeyCode.Alpha3] = new MovementExecutorGun(FindObjectOfType<MovementExecutor>()),
        [KeyCode.Alpha4] = new GraphArrangerGun(FindObjectOfType<GraphArranger>())
      };
      activeGun = guns.First().Value;
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
        activeGun.OnMoveDown(transform, attachedCamera);
      }
      else if (Input.GetButtonDown("Fire2"))
      {
        activeGun.OnRightClick(attachedCamera);
      }
    }

    private void HandleChangeGun()
    {
      var pressedGunKey = guns
          .Keys
          .FirstOrDefault(Input.GetKeyDown);

      if (pressedGunKey == KeyCode.None) 
        return;

      activeGun.OnSwitchedAway();
      activeGun = guns[pressedGunKey];
    }
  }
}
