using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player.Guns
{
    public class GunController : MonoBehaviour
    {
        private Material nodeMaterial;
        private IDictionary<KeyCode, IGun> guns;
        private IGun activeGun;
        private void Start()
        {
            nodeMaterial = Resources.Load<Material>("Materials/Node Material");
            guns = new Dictionary<KeyCode, IGun>
            {
                [KeyCode.Alpha1] = new NodeGun(nodeMaterial),
                [KeyCode.Alpha2] = new MovementExecutorGun(FindObjectOfType<MovementExecutor>())
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
            if (Input.GetButtonDown("Move"))
            {
                activeGun.OnMoveDown(transform);
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