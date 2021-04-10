using System.Collections;
using UnityEngine;

public class MusicAudioController : MonoBehaviour
{
    [SerializeField] private float fadeTime;
    [SerializeField] private AudioClip themeMusic;
    [SerializeField] private AudioClip shortLoopThree;
    [SerializeField] private AudioClip platformerThemeOne;
    [SerializeField] private AudioClip shortLoopSix;
    [SerializeField] private AudioClip platformerThemeEleven;
    [SerializeField] private AudioClip platformerThemeFive;
    [SerializeField] private AudioClip battleThemeOne;
    [SerializeField] private AudioClip battleThemeThree;

    private AudioSource _audioSource;

    private Coroutine _fadeInRoutine;
    private Coroutine _fadeOutThenInRoutine;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.ignoreListenerPause = true;
    }

    public void FadeInThemeMusic()
    {
        CancelCurrentFade();
        _fadeInRoutine = StartCoroutine(FadeInMusic(themeMusic));
    }

    public void FadeToShortLoopThree() => FadeOutThenIn(shortLoopThree);
    public void FadeToShortLoopSix() => FadeOutThenIn(shortLoopSix);
    public void FadeToPlatformerThemeOne() => FadeOutThenIn(platformerThemeOne);
    public void FadeToPlatformerThemeFive() => FadeOutThenIn(platformerThemeFive);
    public void FadeToPlatformerThemeEleven() => FadeOutThenIn(platformerThemeEleven);
    public void FadeToBattleThemeOne() => FadeOutThenIn(battleThemeOne);
    public void FadeToBattleThemeThree() => FadeOutThenIn(battleThemeThree);


    private void FadeOutThenIn(AudioClip fadeToClip)
    {
        if (_fadeOutThenInRoutine != null)
            StopCoroutine(_fadeOutThenInRoutine);
        
        _fadeOutThenInRoutine = StartCoroutine(FadeOutThenInRoutine(fadeToClip));
    }

    private IEnumerator FadeOutThenInRoutine(AudioClip fadeToClip)
    {
        float currentVolume = _audioSource.volume;
        float timer = 0;
        while (timer<fadeTime)
        {
            _audioSource.volume = Mathf.Lerp(currentVolume, 0, timer / fadeTime);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        
        _audioSource.volume = 0;
        _audioSource.clip = fadeToClip;
        _audioSource.Play();
        timer = 0;
        while (timer < fadeTime)
        {
            _audioSource.volume = Mathf.Lerp(0, 1, timer / fadeTime);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
    }
    private IEnumerator FadeInMusic(AudioClip audioClip)
    {
        _audioSource.volume = 0;
        _audioSource.clip = audioClip;
        _audioSource.Play();
        float timer = 0;
        while (timer < fadeTime)
        {
            _audioSource.volume = Mathf.Lerp(0, 1, timer / fadeTime);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    private void CancelCurrentFade()
    {
        if(_fadeInRoutine != null)
            StopCoroutine(_fadeInRoutine);
    }
    
}
