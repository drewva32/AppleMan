using Unity.Mathematics;
using UnityEngine;

public class FlyingMelon : MonoBehaviour, IHaveAnimationEndEvent, IPooledObject
{
    [SerializeField] private float moveSpeed = 300;
    [SerializeField] private float damage;
    
    
    private Rigidbody2D _rb;
    private Animator _animator;
    private static readonly int Explode = Animator.StringToHash("explode");
    private Transform _gFX;
    private Quaternion _startRotation;
    private Collider2D _collider;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _gFX = _animator.transform;
        _startRotation = transform.rotation;
        _collider = GetComponent<Collider2D>();
    }

    private void OnDisable()
    {
        transform.rotation = quaternion.Euler(0,0,180);
        _rb.velocity = Vector2.zero;
        _gFX.transform.localRotation = Quaternion.Euler(0,0,224);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerHealthController>();
        if(player)
            player.TakeDamage();
        _animator.SetTrigger(Explode);
        _collider.enabled = false;
        _gFX.transform.rotation = Quaternion.Euler(0, 0, 0);
        _rb.velocity = Vector2.zero;
    }

    public void OnAnimationEnd()
    {
        gameObject.SetActive(false);
    }

    public void OnObjectSpawn()
    {
        _rb.velocity = transform.up * moveSpeed;
        _collider.enabled = true;
    }
}
