using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarrot : EnemyBase
{
    [SerializeField]
    private int _hitPoints = 2;

    private void Start()
    {
        SetupStats(true, _hitPoints);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Player")
        {
            other.collider.GetComponent<PlayerHealthController>().TakeDamage();
        }
    }
}
