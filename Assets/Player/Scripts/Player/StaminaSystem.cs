using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    [SerializeField] private Player _player;
    [Space(16)]
    [SerializeField] private float _idleStaminaRegen;
    [SerializeField] private float _normalStaminaRegen;
    [SerializeField] private float _crouchStaminaRegen;
    [SerializeField] private float _sprintStaminaConsumption;
    [SerializeField] private float _jumpStaminaConsumption;
    [Space(16)]
    [SerializeField] private float _tirednessRatio;

    [HideInInspector] public float stamina { get; private set; }
    [HideInInspector] public bool isTired { get; private set; }

    private void Start()
    {
        stamina = 100.0f;
    }

    private void Update()
    {
        if (_player.movementController.isGrounded)
        {
            if (_player.movementController.isMoving)
            {
                if (_player.movementController.speedType == MovementController.SpeedType.SPRINTING)
                {
                    stamina -= _sprintStaminaConsumption * Time.deltaTime;
                }
                else if (_player.movementController.speedType == MovementController.SpeedType.NORMAL)
                {
                    stamina += _normalStaminaRegen * Time.deltaTime;
                }
                else
                {
                    stamina += _crouchStaminaRegen * Time.deltaTime;
                }
            }
            else
            {
                stamina += _idleStaminaRegen * Time.deltaTime;
            }
        }


        if (stamina > 100.0f)
        {
            stamina = 100.0f;
        }
        else if (stamina <= 0.0f)
        {
            stamina = 0.0f;

            isTired = true;
        }

        if (isTired && stamina >= 100.0f * _tirednessRatio)
        {
            isTired = false;
        }
    }

    public bool Jump()
    {
        if (stamina >= _jumpStaminaConsumption && !isTired)
        {
            stamina -= _jumpStaminaConsumption;

            return true;
        }
        else
        {
            return false;
        }
    }
}
