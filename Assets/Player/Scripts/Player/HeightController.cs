using UnityEngine;
using UnityEngine.InputSystem;

public class HeightController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [Space(16)]
    [SerializeField] private float _crouchHeightMultiplier;
    [SerializeField] private float _crouchHeightChangeSpeed;

    private float _normalHeight;
    private float _crouchHeight;

    [HideInInspector] public Vector3 cameraHolderOffset;

    private InputAction crouchAction;

    private void Start()
    {
        crouchAction = InputSystem.actions.FindAction("Crouch");

        _normalHeight = _player.characterControllerComponent.height;
        _crouchHeight = _normalHeight * _crouchHeightMultiplier;
    }

    private void Update()
    {
        if (crouchAction.IsPressed() && _player.movementController.isGrounded)
        {
            _player.characterControllerComponent.height = Mathf.Lerp(_player.characterControllerComponent.height, _crouchHeight, _crouchHeightChangeSpeed * Time.deltaTime);
        }
        else if (!_player.movementController.isTouchingCeiling)
        {
            _player.characterControllerComponent.height = Mathf.Lerp(_player.characterControllerComponent.height, _normalHeight, _crouchHeightChangeSpeed * Time.deltaTime);
        }

        _player.characterControllerComponent.center = Vector3.up * _player.characterControllerComponent.height / 2.0f;
        
        _player.cameraHolderTransform.localPosition = Vector3.up * _player.characterControllerComponent.height * 0.9375f + cameraHolderOffset;
    }
}
