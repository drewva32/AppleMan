using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyStateController : MonoBehaviour
{
    public EnemyAttackState EnemyAttackState { get; private set; }
    public EnemyTakeHitState EnemyTakeHitState { get; private set; }
    public EnemyDieState EnemyDieState { get; private set; }
    public EnemyChaseState EnemyChaseState { get; private set; }
    public EnemyWalkState EnemyWalkState { get; private set; }
    public StateMachine StateMachine { get; private set; }
    public Animator Animator => _animator;
    public WalkingController Walkingcontorller => _walkingController;
    public Transform Player => _player;

    [SerializeField]
    private Transform _player;

    private Animator _animator;
    private WalkingController _walkingController;

    private void Awake()
    {
        EnemyAttackState = new EnemyAttackState(this);
        EnemyTakeHitState = new EnemyTakeHitState(this);
        EnemyDieState = new EnemyDieState(this);
        EnemyChaseState = new EnemyChaseState(this);
        EnemyWalkState = new EnemyWalkState(this);
        StateMachine = new StateMachine();

        _animator = GetComponent<Animator>();
        _walkingController = GetComponentInParent<WalkingController>();

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
}
