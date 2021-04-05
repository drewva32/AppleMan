using UnityEngine;

public class EnemyStateController : MonoBehaviour, IPlayerInteractions
{
    public EnemyAttackState EnemyAttackState { get; private set; }
    public EnemyTakeHitState EnemyTakeHitState { get; private set; }
    public EnemyDieState EnemyDieState { get; private set; }
    public EnemyChaseState EnemyChaseState { get; private set; }
    public EnemyWalkState EnemyWalkState { get; private set; }
    public EnemyProtectState EnemyProtectState { get; private set; }
    public StateMachine StateMachine { get; private set; }
    public Animator Animator => _animator;
    public WalkingController Walkingcontorller => _walkingController;
    public Transform Player => _player;

    [SerializeField]
    private Transform _player;

    private Animator _animator;
    private WalkingController _walkingController;
    private EnemyBase _enemy;

    private void Awake()
    {
        EnemyAttackState = new EnemyAttackState(this);
        EnemyTakeHitState = new EnemyTakeHitState(this);
        EnemyDieState = new EnemyDieState(this);
        EnemyChaseState = new EnemyChaseState(this);
        EnemyWalkState = new EnemyWalkState(this);
        EnemyProtectState = new EnemyProtectState(this);
        StateMachine = new StateMachine();

        _animator = GetComponentInChildren<Animator>();
        _walkingController = GetComponent<WalkingController>();
        _enemy = GetComponent<EnemyBase>();

    }
    // Start is called before the first frame update
    void Start()
    {
        StateMachine.Init(EnemyWalkState);
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.FixedLogicUpdate();
    }

    public void AnimationTrigger()
    {
        StateMachine.CurrentState.OnAnimationEnd();
    }

    public void TakeHit()
    {
        StateMachine.CurrentState.TakeHit();
    }

    public void TakePunch(int damageAmount)
    {
        _walkingController.RB.velocity = Vector2.zero;
        Vector3 playerLocation = (_walkingController._player.position - _walkingController.transform.position);
        Debug.Log(playerLocation.normalized);
        // Apply damage
        _enemy.TakeDamage(damageAmount);
        CheckHealth();
        _walkingController.RB.AddForce(new Vector2(-playerLocation.x * 100, 25f));
        StateMachine.CurrentState.TakeHit();
    }

    public void TakeSlide(int damageAmount, Vector3 directionToPlayer)
    {
        // Apply Damage
        _enemy.TakeDamage(damageAmount);
        CheckHealth();
        _walkingController.RB.AddForce(new Vector2(-directionToPlayer.x * 250, 0));
        StateMachine.CurrentState.TakeHit();
    }

    private void CheckHealth()
    {
        if (_enemy.CurrentHealth < 0)
            StateMachine.ChangeState(EnemyDieState);
    }
}


/* This came from the TakePunch Method and I am not sure we need it the current way the code is setup.

bool playerOnLeft;
playerOnLeft = (Vector3.Dot(directionToPlayer.normalized, Vector3.right) > 1) ? true : false;
Debug.Log(playerOnLeft);
_walkingController.RB.velocity = Vector2.zero;
if (playerOnLeft)
{
    _walkingController.RB.AddForce(new Vector2(directionToPlayer.x * 500, 25f));
}
else
{
    _walkingController.RB.AddForce(new Vector2(-directionToPlayer.x * 500, 25f));
}
*/