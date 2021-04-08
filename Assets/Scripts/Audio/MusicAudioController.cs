using System.Collections;
using UnityEngine;

public class MusicAudioController : MonoBehaviour
{
    [SerializeField] private float fadeTime;
    [SerializeField] private AudioClip themeMusic;

    private AudioSource _audioSource;

    private Coroutine _fadeRoutine;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.ignoreListenerPause = true;
    }

    public void FadeInThemeMusic()
    {
        CancelCurrentFade();
        _fadeRoutine = StartCoroutine(FadeInMusic(themeMusic));
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
        if(_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);
    }
}
