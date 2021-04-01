using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WalkingController : MonoBehaviour
{
    [SerializeField]
    private float _visionDistatnce = 0;
    [SerializeField]
    private float _speed = 2.0f;
    [SerializeField]
    private float _chaseSpeed = 4.0f;
    [SerializeField]
    private Transform _surfaceChecker;
    [SerializeField]
    private LayerMask _turnLayerMask;
    [SerializeField]
    private LayerMask _playerLayerMask;
    [SerializeField]
    private float platformRayDistance = 0.1f;
    [SerializeField]
    private float _wallCheckDistance = 0.2f;
    [SerializeField] [Tooltip("1 if character is facing right; -1 otjerwise")]
    private int _facingDirection = 1;


    public Transform WallCheck => _surfaceChecker;
    public float Speed => _currentSpeed;
    public float ChaseDistance => _visionDistatnce;
    public LayerMask PlayerLayer => _playerLayerMask;
    public Rigidbody2D RB => _rb;

    
    private bool _timeToTurn = false;
    private bool _onPlatform = true;
    private float _currentSpeed;
    private Rigidbody2D _rb;
    private Vector3 vectorFacingDirection;


    public Transform _player;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _currentSpeed = _speed;
        vectorFacingDirection = new Vector3(_facingDirection, 0, 0);
    }

    public void Walk()
    {
        if (_facingDirection == 1)
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
        _timeToTurn = Physics2D.Raycast(_surfaceChecker.position, vectorFacingDirection, _wallCheckDistance, _turnLayerMask);
        _onPlatform = Physics2D.Raycast(_surfaceChecker.position, Vector3.down, platformRayDistance, _turnLayerMask);
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
        // Draw ray for vision
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(WallCheck.position, new Vector3(_facingDirection * _visionDistatnce, 0, 0));
        // Draw ray to ground to check for platform turning
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_surfaceChecker.position, new Vector3(0, -platformRayDistance, 0));
        // Draw ray to check for wall
        Gizmos.color = Color.black;
        Gizmos.DrawRay(_surfaceChecker.position, new Vector3(_facingDirection * _wallCheckDistance, 0, 0));
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        _facingDirection *= -1;

        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
