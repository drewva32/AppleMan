using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WalkingController : MonoBehaviour
{
    [SerializeField]
    private float _chaseDistatnce = 0;
    [SerializeField]
    private float _speed = 2.0f;
    [SerializeField]
    private float _chaseSpeed = 4.0f;
    [SerializeField]
    private Transform _wallCheck;
    [SerializeField]
    private LayerMask _turnLayerMask;
    [SerializeField]
    private LayerMask _playerLayerMask;
    [SerializeField]
    private float platformRayDistance = 0.1f;


    public Transform WallCheck => _wallCheck;
    public float Speed => _currentSpeed;
    public float ChaseDistance => _chaseDistatnce;
    public LayerMask PlayerLayer => _playerLayerMask;

    private float _wallCheckRadius = 0.2f;
    private bool _timeToTurn = false;
    private bool _onPlatform = true;
    private bool _isFacingRight = false;
    private float _currentSpeed;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _currentSpeed = _speed;
    }

    public void Walk()
    {
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
        _timeToTurn = Physics2D.OverlapCircle(_wallCheck.position,  _wallCheckRadius, _turnLayerMask);
        _onPlatform = Physics2D.Raycast(_wallCheck.position, Vector3.down, platformRayDistance, _turnLayerMask);
        if(_timeToTurn || !_onPlatform)
            Flip();
    }

    public void ChangeSpeed()
    {
        if (_currentSpeed == _speed)
            _currentSpeed = _chaseSpeed;
        else
            _currentSpeed = _speed;
    }

    private void OnDrawGizmos()
    {
        // Draw ray to ground to check for platform turning
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_wallCheck.position, new Vector3(0, -platformRayDistance, 0));
        // Draw wall sphere to check for turning
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_wallCheck.position, _wallCheckRadius);
        // Draw ray for vision
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(WallCheck.position, new Vector3(-transform.localScale.x * _chaseDistatnce, 0, 0));
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
