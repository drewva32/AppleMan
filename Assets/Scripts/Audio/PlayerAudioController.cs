using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip stepOneSound;
    [SerializeField] private AudioClip stepTwoSound;
    [SerializeField] private AudioClip jumpUpSound;
    [SerializeField] private AudioClip landSound;
    [SerializeField] private AudioClip punchSound;
    [SerializeField] private AudioClip groundSlideSound;
    [SerializeField] private AudioClip wallSlideSound;
    [SerializeField] private AudioClip idleSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioSource oneShotAudioSource;
    [SerializeField] private AudioSource cancelableAudioSource;
    [SerializeField] private AudioSource loopingAudioSource;
    
    
   

    private void Awake()
    {
        oneShotAudioSource.playOnAwake = false;
        loopingAudioSource.playOnAwake = false;
        loopingAudioSource.loop = true;
        oneShotAudioSource.loop = false;
    }

    private void PlaySound(AudioClip clip) => oneShotAudioSource.PlayOneShot(clip);
    private void PlayCancelableSound(AudioClip clip) => cancelableAudioSource.PlayOneShot(clip);
    private void PlayLoopingSound(AudioClip clip)
    {
        loopingAudioSource.clip = clip;
        loopingAudioSource.Play();
    }

    public void CancelSound() => cancelableAudioSource.Stop();
    public void CancelLoopingSound() => loopingAudioSource.Stop();

    public void PlayStepOneSound() => PlaySound(stepOneSound);
    public void PlayStepTwoSound() => PlaySound(stepTwoSound);
    public void PlayJumpUpSound() => PlayCancelableSound(jumpUpSound);
    public void PlayLandSound() => PlaySound(landSound);
    public void PlayPunchSound() => PlaySound(punchSound);
    public void PlayGroundSlideSound() => PlaySound(groundSlideSound);
    public void PlayWallSlideSound() => PlayLoopingSound(wallSlideSound);
    public void PlayIdleSound() => PlaySound(idleSound);
    public void PlayHurtSound() => PlaySound(hurtSound);
    public void PlayDeathSound() => PlaySound(deathSound);


    public void SetIsLooping(bool isLooping)
    {
        oneShotAudioSource.loop = isLooping;
    }
}
