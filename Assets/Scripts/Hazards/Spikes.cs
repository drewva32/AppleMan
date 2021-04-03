using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D other)
    {
        var spikeHit = other.gameObject.GetComponent<ITakeSpikeDamage>();
        spikeHit?.TakeSpikeDamage();
    }
}
