using UnityEngine;

public class AnimationEventHelper : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
    }

    private void AnimationTrigger() => _player.AnimationTrigger();

    private void AnimationFinishTrigger() => _player.AnimationFinishTrigger();
}
