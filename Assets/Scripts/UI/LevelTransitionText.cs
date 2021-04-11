using TMPro;
using UnityEngine;

public class LevelTransitionText : MonoBehaviour
{
    [SerializeField][Multiline] private string transitionText;
    [SerializeField] private TextMeshProUGUI tmpText;
    
    private Animator _animator;
    private bool _init;
    private bool _hasTransitioned;
    private static readonly int Transition1 = Animator.StringToHash("transition");

    private void Awake()
    {
        _animator = tmpText.GetComponent<Animator>();
    }

    public void Transition(string text)
    {
        tmpText.text = text;
        _animator.SetTrigger(Transition1);
    }

    private void OnEnable()
    {
        if (_init && !_hasTransitioned)
        {
            Transition(transitionText);
            _hasTransitioned = true;
        }

        _init = true;
    }
}
