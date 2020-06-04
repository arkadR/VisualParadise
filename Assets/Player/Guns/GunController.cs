using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player.Guns
{
    public class GunController : MonoBehaviour
    {
        private GunTypeChange gunTypeChange;
        private IDictionary<KeyCode, IGun> guns;
        private Camera attachedCamera;
        private GraphLoader graphLoader;
        private IGun _activeGun;
        private IGun activeGun
        {
            get => _activeGun;
            set
            {
                gunTypeChange.ChangeGunType(value.GetGunName());
                _activeGun = value;
            }
        }

        private void Start()
        {
            attachedCamera = Camera.main;
            graphLoader = FindObjectOfType<GraphLoader>();
            gunTypeChange = FindObjectOfType<GunTypeChange>();
            var nodeMaterial = Resources.Load<Material>("Materials/Node Material");
            var edgeMaterial = Resources.Load<Material>("Materials/Edge Material");

            guns = new Dictionary<KeyCode, IGun>
            {
                [KeyCode.Alpha1] = new NodeGun(graphLoader, nodeMaterial),
                [KeyCode.Alpha2] = new EdgeGun(graphLoader, edgeMaterial),
                [KeyCode.Alpha3] = new MovementExecutorGun(FindObjectOfType<MovementExecutor>()),
                [KeyCode.Alpha4] = new GraphArrangerGun(FindObjectOfType<GraphArranger>())
            };
            activeGun = guns.First().Value;
        }

        private void Update()
        {
            HandleChangeGun();
            HandleFire();
        }

        private void HandleFire()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                activeGun.OnMoveDown(transform, attachedCamera);
            }
        }

        private void HandleChangeGun()
        {
            var pressedGunKey = guns
                .Keys
                .FirstOrDefault(Input.GetKeyDown);

            if (pressedGunKey == KeyCode.None) return;
            activeGun.OnSwitchedAway();
            activeGun = guns[pressedGunKey];
        }
    }
}