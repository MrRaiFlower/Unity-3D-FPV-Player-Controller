using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    public enum SpeedType
    {
        SPRINTING,
        CROCUHING,
        NORMAL
    }

    [SerializeField] private Player _player;
    [Space(16)]
    [SerializeField] private float normalSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float sprintSpeed;
    [Space(16)]
    [SerializeField] private float _groundAcceleration;
    [SerializeField] private float _airAceleration;
    [SerializeField] private float _groundDrag;
    [SerializeField] private float _airDrag;
    [SerializeField] private float _jumpSpeed;
    
    [HideInInspector] public bool isGrounded {get; private set;}
    [HideInInspector] public bool wasGrounded {get; private set;}
    [HideInInspector] public bool hasGrounded {get; private set;}

    [HideInInspector] public bool isTouchingCeiling {get; private set;}
    [HideInInspector] public bool wasTouchingCeiling {get; private set;}
    [HideInInspector] public bool hasTouchedCeiling {get; private set;}

    private Vector2 _moveDirectionInput;
    private Vector3 _moveDirection;

    [HideInInspector] public bool isMoving {get; private set;}

    private float _acceleration;
    private float _horizontalSpeed;

    [HideInInspector] public float speedRatio {get; private set;}
    [HideInInspector] public float crouchSpeedRatio {get; private set;}
    [HideInInspector] public float sprintSpeedRatio {get; private set;}
    [HideInInspector] public float normalSpeedRatio {get; private set;}
    [HideInInspector] public SpeedType speedType {get; private set;}

    [HideInInspector] public Vector3 horizontalvelocity {get; private set;}
    [HideInInspector] public Vector3 verticalVelocity {get; private set;}

    private Vector3 _velocity;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _crouchAction;
    private InputAction _sprintAction;

    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _crouchAction = InputSystem.actions.FindAction("Crouch");
        _sprintAction = InputSystem.actions.FindAction("Sprint");

        crouchSpeedRatio = crouchSpeed / normalSpeed;
        sprintSpeedRatio = sprintSpeed / normalSpeed;
        normalSpeedRatio = 1.0f;
    }

    private void Update()
    {
        CheckGround();
        CheckCeiling();

        CalculateMoveDirection();

        CalculateHorizontalVelocity();
        CalculateVerticalVelocity();
        
        ComposeVelocity();

        Move();
    }

    private void CalculateMoveDirection()
    {
        _moveDirectionInput = _moveAction.ReadValue<Vector2>();

        isMoving = _moveDirectionInput.sqrMagnitude != 0.0f;

        if (isMoving)
        {
            if (_crouchAction.IsPressed())
            {
                _horizontalSpeed = crouchSpeed;
                speedType = SpeedType.CROCUHING;
            }
            else if (_sprintAction.IsPressed() && _player.staminaSystem.stamina >= 0.0f && !_player.staminaSystem.isTired)
            {
                _horizontalSpeed = sprintSpeed;
                speedType = SpeedType.SPRINTING;
            }
            else
            {
                _horizontalSpeed = normalSpeed;
                speedType = SpeedType.NORMAL;
            }
        }
        else
        {
            _horizontalSpeed = 0.0f;
        }

        if (isMoving)
        {
            _moveDirection = (_player.mainTransform.right * _moveDirectionInput.x + _player.mainTransform.forward * _moveDirectionInput.y).normalized * _horizontalSpeed;
            speedRatio = _horizontalSpeed / normalSpeed;
        }
        else if (_horizontalSpeed == 0.0f)
        {
            _moveDirection = Vector3.zero;
        }
    }

    private void CalculateHorizontalVelocity()
    {
        if (isGrounded)
        {
            if (isMoving)
            {
                _acceleration = _groundAcceleration;
            }
            else
            {
                _acceleration = _groundDrag;
            }
        }
        else
        {
            if (isMoving)
            {
                _acceleration = _airAceleration;
            }
            else
            {
                _acceleration = _airDrag;
            }
        }

        horizontalvelocity = Vector3.Lerp(horizontalvelocity, _moveDirection, _acceleration * Time.deltaTime);
    }

    private void CalculateVerticalVelocity()
    {
        if (_jumpAction.WasPressedThisFrame() && isGrounded && _player.staminaSystem.Jump())
        {
            verticalVelocity = Vector3.up * _jumpSpeed;

            _player.playerAudioController.PlayJumpSound();
        }
        else if (hasTouchedCeiling)
        {
            verticalVelocity = Vector3.zero;
        }
        else if (isGrounded && verticalVelocity.y < 0.0f)
        {
            verticalVelocity = Vector3.down * 0.1f;
        }
        else
        {
            verticalVelocity += Vector3.down * 9.81f * Time.deltaTime;
        }
    }

    private void ComposeVelocity()
    {
        _velocity = horizontalvelocity + verticalVelocity;
    }

    private void Move()
    {
        _player.characterControllerComponent.Move(_velocity * Time.deltaTime);
    }

    private void CheckGround()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(_player.mainTransform.position + Vector3.up * _player.characterControllerComponent.radius * 0.475f, _player.characterControllerComponent.radius * 0.95f, ~_player.layerMask);
        hasGrounded = !wasGrounded && isGrounded;

        if (hasGrounded && -verticalVelocity.y > _jumpSpeed * 0.95f)
        {
            _player.playerAudioController.PlayLandingSound();
        }
    }

    private void CheckCeiling()
    {
        wasTouchingCeiling = isTouchingCeiling;
        isTouchingCeiling = Physics.CheckSphere(_player.mainTransform.position + Vector3.up * (_player.characterControllerComponent.height -(_player.characterControllerComponent.radius * 0.475f)), _player.characterControllerComponent.radius * 0.95f, ~_player.layerMask);
        hasTouchedCeiling = !wasTouchingCeiling && isTouchingCeiling;
    }
}
