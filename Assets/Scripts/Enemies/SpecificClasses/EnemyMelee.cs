using UnityEngine;

public class EnemyMelee : EnemyBase
{
    [SerializeField]
    private int _hitPoints = 2;
    [SerializeField]
    private float _meleeDistance = 0.5f;
    [SerializeField]
    private Transform _meleeVector;

    public float MeleeDistance => _meleeDistance;
    public Transform MeleeChecker => _meleeVector;


    private void Start()
    {
        SetupStats(true, _hitPoints);
    }

    private void OnDrawGizmos()
    {
        // Draw circle for Melee
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_meleeVector.position, _meleeDistance);
    }
}
