using UnityEngine;

public class FOVController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [Space(16)]
    [SerializeField] private float _factor;
    [SerializeField] private float _changeSpeed;

    private float _defaultFOV;
    private float _slowFOV;
    private float _fastFOV;

    private void Start()
    {
        _defaultFOV = _player.mainCamera.fieldOfView;
        _slowFOV = _defaultFOV * (1.0f - _factor);
        _fastFOV = _defaultFOV * (1.0f + _factor);
    }

    private void Update()
    {
        switch (_player.movementController.speedType)
        {
            case MovementController.SpeedType.CROCUHING:
                _player.mainCamera.fieldOfView = Mathf.Lerp(_player.mainCamera.fieldOfView, _slowFOV, _changeSpeed * Time.deltaTime);
                break;
            case MovementController.SpeedType.SPRINTING:
                _player.mainCamera.fieldOfView = Mathf.Lerp(_player.mainCamera.fieldOfView, _fastFOV, _changeSpeed * Time.deltaTime);
                break;
            case MovementController.SpeedType.NORMAL:
                _player.mainCamera.fieldOfView = Mathf.Lerp(_player.mainCamera.fieldOfView, _defaultFOV, _changeSpeed * Time.deltaTime);
                break;
        }
    }
}
