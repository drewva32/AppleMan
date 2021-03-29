using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WalkingController : MonoBehaviour
{
    [SerializeField]
    private float _chaseDistatnce = 0;
    // speed here
    [SerializeField]
    private float _speed = 2.0f;
    [SerializeField]
    private Transform _wallCheck;
    [SerializeField]
    private LayerMask turnLayerMask;
    [SerializeField]
    private float platformRayDistance = 0.1f;


    public Transform WallCheck => _wallCheck;
    public float Speed => _speed;
    public float ChaseDistance => _chaseDistatnce;


    private float _wallCheckRadius = 0.2f;
    private bool _timeToTurn = false;
    private bool _onPlatform = true;
    private bool _isFacingRight = false;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Walk()
    {
        // Walk functionality here
        if (_isFacingRight)
        {
            _rb.velocity = new Vector2(_speed, _rb.velocity.y);
        }
        else
        {
            _rb.velocity = new Vector2(-_speed, _rb.velocity.y);
        }
    }

    public void CheckDirection ()
    {
        // Raycast to look for wall
        _timeToTurn = Physics2D.OverlapCircle(_wallCheck.position,  _wallCheckRadius, turnLayerMask);
        _onPlatform = Physics2D.Raycast(_wallCheck.position, Vector3.down, platformRayDistance);
        if(_timeToTurn || !_onPlatform)
        {
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_wallCheck.position, new Vector3(0, -platformRayDistance, 0));
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_wallCheck.position, _wallCheckRadius);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        _isFacingRight = !_isFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
