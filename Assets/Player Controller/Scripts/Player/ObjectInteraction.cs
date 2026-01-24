using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] private GameObject cameraObject;

    [SerializeField] private float range;

    private InputAction interactAction;
    
    private void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    private void Update()
    {
        if (interactAction.WasPressedThisFrame())
        {
            RaycastHit hit;
            Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward, out hit, range);
            if (hit.collider.gameObject.TryGetComponent(out InteractableObject interactableObject))
            {
                interactableObject.Interact();
            }
        }
    }
}
