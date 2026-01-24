using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioSource jumpSoundComponent;
    [SerializeField] private AudioSource landngSoundComponent;
    [SerializeField] private AudioSource flashlightSoundComponent;
    [SerializeField] private AudioSource footstepSoundComponent;

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

    public void PlayFootstepSound(float volume)
    {
        footstepSoundComponent.volume = volume;
        footstepSoundComponent.Play();
    }
}
