using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private CollectiblesAudioController coinAudio;
    [SerializeField] private CollectiblesAudioController oneUPAudio;
    [SerializeField] private CollectiblesAudioController healthPickupAudio;

    public CollectiblesAudioController CoinAudio => coinAudio;
    public CollectiblesAudioController OneUpAudio => oneUPAudio;
    public CollectiblesAudioController HealthPickupAudio => healthPickupAudio;



    private static AudioManager _instance;

    public static AudioManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
            _instance = this;
    }

    private void OnDestroy() => _instance = null;
}
