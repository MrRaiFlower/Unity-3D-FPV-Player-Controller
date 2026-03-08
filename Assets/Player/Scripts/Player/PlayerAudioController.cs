using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [Space(16)]
    [SerializeField] private AudioSource footstepSoundComponent;
    [Space(16)]
    [SerializeField] private AudioSource jumpSoundComponent;
    [SerializeField] private AudioSource landngSoundComponent;
    [Space(16)]
    [SerializeField] private AudioSource flashlightSoundComponent;

    public void PlayFootstepSound()
    {
        switch (_player.movementController.speedType)
        {
            case MovementController.SpeedType.CROCUHING: footstepSoundComponent.volume = 0.5f; break;
            case MovementController.SpeedType.SPRINTING: footstepSoundComponent.volume = 1.0f; break;
            case MovementController.SpeedType.NORMAL: footstepSoundComponent.volume = 0.75f; break;
        }
        footstepSoundComponent.Play();
    }

    public void PlayJumpSound()
    {
        jumpSoundComponent.Play();
    }

    public void PlayLandingSound()
    {
        landngSoundComponent.Play();
    }

    public void PlayFlashlightSound()
    {
        flashlightSoundComponent.Play();
    }
}
