using UnityEngine;

namespace Assets.Scripts.Tools
{
  class ToolgunRecoil : MonoBehaviour
  {
    private const float _maxRecoilX = -2f;
    private const float _maxRecoilY = 2f;
    private const float _recoilSpeed = 15f;
    private float _recoil = 0f;
    private Quaternion _defaultRotation;

    public void AddRecoil() => _recoil += 0.1f;

    public void Start() => _defaultRotation = transform.rotation;

    public void FixedUpdate()
    {
      if (_recoil > 0)
      {
        var recoilQuaternion = Quaternion.Euler(_maxRecoilX * Random.value, _maxRecoilY * Random.value, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, recoilQuaternion * transform.localRotation, Time.deltaTime * _recoilSpeed);
        _recoil -= Time.deltaTime;
      }
      else
      {
        _recoil = 0;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, _defaultRotation, Time.deltaTime * _recoilSpeed / 2);
      }
    }
  }
}
