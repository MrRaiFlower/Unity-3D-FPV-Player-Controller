using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectInteractionController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [Space(16)]
    [SerializeField] private float _interactionRange;

    private InputAction _interactAction;
    
    private void Start()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
    }

    private void Update()
    {
        if (_interactAction.WasPressedThisFrame())
        {
            RaycastHit hit;
            if (Physics.Raycast(_player.cameraTransform.position, _player.cameraTransform.forward, out hit, _interactionRange))
            {
                if (hit.collider.gameObject.TryGetComponent(out InteractableObject interactableObject))
                {
                    interactableObject.Interact();
                }
            }
        }
    }
}
