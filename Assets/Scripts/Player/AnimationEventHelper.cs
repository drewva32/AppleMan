using UnityEngine;

public class AnimationEventHelper : MonoBehaviour
{
    private Player _player;
    private PunchHitMarker _punchHitMarker;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _punchHitMarker = GetComponentInChildren<PunchHitMarker>();
    }

    private void AnimationTrigger() => _player.AnimationTrigger();

    private void AnimationFinishTrigger() => _player.AnimationFinishTrigger();

    private void OnPunchTriggered() => _punchHitMarker.HitMarker();
}
