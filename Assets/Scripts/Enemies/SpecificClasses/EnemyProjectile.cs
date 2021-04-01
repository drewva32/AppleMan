using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField]
    private float _speed = 50f;
    [SerializeField]
    private float _lifeTimer = 2f;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        StartCoroutine(LifeCountDown(_lifeTimer));
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = transform.right * _speed;
    }


    IEnumerator LifeCountDown(float timer)
    {
        Debug.Log("Life Satarted");
        float temp = timer;
        temp -= 0.5f;
        if (temp < 0)
            Destroy(this);
        yield return new WaitForSeconds(1f);
    }
}
