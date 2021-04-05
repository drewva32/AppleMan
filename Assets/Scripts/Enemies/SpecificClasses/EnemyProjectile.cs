using System.Collections;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField]
    private float _speed = 50f;
    [SerializeField]
    private float _lifeTimer = 2f;
    [SerializeField]
    private GameObject _grapeSplat;
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

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerHealthController pHealth = other.collider.GetComponent<PlayerHealthController>();
            Debug.Log("NOW YOU DIE!!!!");
            pHealth.TakeDamage();
        }
        GameObject splat = Instantiate(_grapeSplat, this.transform.position, transform.rotation);
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
