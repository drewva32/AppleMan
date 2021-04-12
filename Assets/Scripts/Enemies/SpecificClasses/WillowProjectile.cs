using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillowProjectile : MonoBehaviour
{
    [SerializeField]
    private float _speed = 50f;
    [SerializeField]
    private float _lifeTimer = 2f;
    [SerializeField]
    private GameObject explosionEffect;
    private Rigidbody2D _rb;
    private Transform _player;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = AppleGameManager.Instance.CurrentPlayerTransform;
        StartCoroutine(LifeCountDown(_lifeTimer));
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 directionToPlayer = _player.position - this.transform.position;
        _rb.velocity = transform.right * directionToPlayer * _speed;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController pHealth = other.collider.GetComponent<PlayerHealthController>();
            Debug.Log("NOW YOU DIE!!!!");
            pHealth.TakeDamage();
        }
        GameObject splat = Instantiate(explosionEffect, this.transform.position, transform.rotation);
        Destroy(this.gameObject);
    }


    IEnumerator LifeCountDown(float timer)
    {
        while (_lifeTimer > 0)
        {
            yield return new WaitForSeconds(1f);
            _lifeTimer--;
        }
        Destroy(this.gameObject);
    }
}
