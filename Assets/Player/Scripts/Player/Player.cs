using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform mainTransform;
    public Transform cameraHolderTransform;
    public Transform cameraTransform;
    [Space(16)]
    public Camera mainCamera;
    [Space(16)]
    public CharacterController characterControllerComponent;
    [Space(16)]
    public LayerMask layerMask;
    [Space(16)]
    public cameraController cameraController;
    public FlashlightController flashlightController;
    public FootstepsAnimator footstepsAnimator;
    public FOVController fOVController;
    public HeightController heightController;
    public MovementController movementController;
    public ObjectInteractionController objectInteractionController;
    public PlayerAudioController playerAudioController;
    public StaminaSystem staminaSystem;
}
