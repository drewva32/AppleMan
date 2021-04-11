using System.Collections.Generic;
using UnityEngine;

public class RespawnOnPlayerDeath : MonoBehaviour
{
    private bool _init;
    private int _lifeID;
    private HealthPickUp[] _healthPickUps;

    private void Awake()
    {
        _healthPickUps = GetComponentsInChildren<HealthPickUp>();
    }

    private void OnEnable()
    {
        if (_init)
        {
            if (_lifeID != AppleGameManager.Instance.lifeID)
            {
                _lifeID = AppleGameManager.Instance.lifeID;
                foreach (var healthPickUp in _healthPickUps)
                {
                    healthPickUp.Reset();
                }
            }
        }

        _init = true;
    }
}
