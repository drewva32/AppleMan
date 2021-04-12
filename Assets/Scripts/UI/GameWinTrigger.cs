using System.Collections;
using UnityEngine;

public class GameWinTrigger : MonoBehaviour
{
    [SerializeField] private GameObject transitionText;
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        transitionText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player)
        {
            if(transitionText != null)
                transitionText.gameObject.SetActive(true);
           
            _animator.SetTrigger("win");
            StartCoroutine(WinGameRoutine());
            //play win music
            // if (AudioManager.Instance != null)
            //     AudioManager.Instance.OneUpAudio.PlayCollectSound();
        }
    }

    private IEnumerator WinGameRoutine()
    {
        yield return new WaitForSeconds(5);
        AppleGameManager.Instance.GameOver();
    }
    
}
