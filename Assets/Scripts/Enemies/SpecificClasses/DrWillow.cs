using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrWillow : EnemyBase
{
    [SerializeField]
    private int _hitPoints = 30;
    [SerializeField]
    private List<GameObject> _projectiles;
    [SerializeField]
    private Transform _throwPosition;

    public string CurrentDecision => _currentDecision;
    public bool DecisionMade => _decisionMade;

    private bool _canMakeDecision = false;
    private Transform _projectileParent;
    private WalkingController _wc;
    private string _currentDecision;
    private bool _decisionMade = false;

    void Start()
    {
        _wc = GetComponent<WalkingController>();

        _projectileParent = GameObject.Find("Projectiles").transform;
        SetupStats(true, _hitPoints);
    }

    // Update is called once per frame
    void Update()
    {
        if (AppleGameManager.Instance.CurrentPlayerTransform.position.x < transform.position.x && _wc.FacingDirection == 1 ||
            AppleGameManager.Instance.CurrentPlayerTransform.position.x > transform.position.x && _wc.FacingDirection == -1)
            _wc.Flip();
        if (_canMakeDecision)
            MakeDecision();
    }

    private void MakeDecision()
    {
        _decisionMade = false;
        switch (PickRandomNumber(1,3))
        {
            case 1:
                _currentDecision = "idle";
                break;
            case 2:
                _currentDecision = "attack";
                break;
            case 3:
                _currentDecision = "move";
                break;
            default:
                MakeDecision();
                break;
        }
    }


    public void Test()
    {
        _decisionMade = false;
        StartCoroutine(DecisionCoolDown(Random.Range(0.5f, 2f)));
    }
    public override void LaunchProjectile()
    {
        GameObject ammo = Instantiate(_projectiles[PickRandomNumber(0, _projectiles.Count)], _throwPosition.position, transform.rotation);
        ammo.transform.parent = _projectileParent.transform;
    }

    private int PickRandomNumber(int min, int max)
    {
        int rand = Random.Range(min, max);
        return rand;
    }

    IEnumerator DecisionCoolDown(float waitTime)
    {
        while (waitTime > 0)
        {
            waitTime -= 0.25f;
            yield return new WaitForSeconds(0.25f);
        }
        _decisionMade = true;
    }
}
