using UnityEngine;
using UnityEngine.InputSystem;

public class cameraController : MonoBehaviour
{
    [SerializeField] Player _player;

    [SerializeField] private float _sensitivity;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _drag;

    private Vector2 _mouseVelocity;
    private Vector2 _cameraVelocity;

    private Vector3 _cameraRotation;

    private InputAction _lookAction;

    private void Start()
    {
        _lookAction = InputSystem.actions.FindAction("Look");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _mouseVelocity = _lookAction.ReadValue<Vector2>() / Time.unscaledDeltaTime;

        if (_mouseVelocity.sqrMagnitude != 0.0f)
        {
            _cameraVelocity = Vector2.Lerp(_cameraVelocity, Vector2.right * (_mouseVelocity.x * _sensitivity / 2.0f * Time.deltaTime) + Vector2.up * (_mouseVelocity.y * _sensitivity * Time.deltaTime), _acceleration * Time.deltaTime);
        }
        else
        {
            _cameraVelocity = Vector2.Lerp(_cameraVelocity, Vector2.zero, _drag * Time.deltaTime);
        }

        _cameraRotation.y += _cameraVelocity.x;

        _cameraRotation.x -= _cameraVelocity.y;
        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, -90.0f, 90.0f);

        _player.mainTransform.rotation = Quaternion.Euler(0.0f, _cameraRotation.y, 0.0f);
        _player.cameraTransform.localRotation = Quaternion.Euler(_cameraRotation.x, 0.0f, 0.0f);
    }
}
