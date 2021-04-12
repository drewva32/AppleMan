using System.Collections;
using UnityEngine;

public class OrangeBoss : EnemyBase
{
    [SerializeField]
    private float _movespeed = 6f;
    [SerializeField]
    private float _jumpPower;
    [SerializeField]
    private int _hitPoints;
    [SerializeField]
    private GameObject orangeRain;
    [SerializeField]
    private Transform rightBounds;
    [SerializeField]
    private Transform leftBounds;

    public bool IsDecisionMade => _decisionMade;
    public string CurrentAttack => _currentAttack;


    private bool _decisionMade = false;
    private Transform _projectileParent;
    private string _currentAttack;
    private WalkingController _wc;

    private void Start()
    {
        SetupStats(true, _hitPoints);
        _projectileParent = GameObject.Find("Projectiles").transform;
        _wc = GetComponent<WalkingController>();
    }

    public string AttackDecicison ()
    {
        string testAttack;
        float randAttack = Random.Range(1, 3);
        if (randAttack <= 1f)
            testAttack = "charge";
        else if (randAttack > 1f && randAttack <= 2)
            testAttack = "focus";
        else
            testAttack = "slam";

        if (testAttack == _currentAttack)
            AttackDecicison();
        else
            _decisionMade = true;

        return _currentAttack;
    }

    public void FocusAttack()
    {
        int randAmount = Random.Range(5, 10);

        StartCoroutine(DropOranges(randAmount));
    }

    public void ChargeAttack()
    {
        _wc.SetVelocity(Vector2.right * _wc.vectorDirection * _movespeed);
    }

    public void SlamAttack()
    {
        _wc.SetVelocity(new Vector2(0.5f * _wc.vectorDirection.x, _jumpPower));
        //_wc.SetVelocity(Vector2.up * _wc.vectorDirection * _jumpPower);
    }

    public void MakeDecision(float waitTime)
    {
        _decisionMade = false;
        StartCoroutine(WaitForDecision(waitTime));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
            other.gameObject.GetComponent<PlayerHealthController>().TakeDamage();
    }

    IEnumerator DropOranges(int orangeCount)
    {
        float randX;
        float randRotation;
        for (int i = 0; i < orangeCount; i++)
        {
            randRotation = Random.Range(-135, -35);
            randX = Random.Range(leftBounds.position.x, rightBounds.position.x);
            GameObject orangeBomb = Instantiate(orangeRain, new Vector3(randX, rightBounds.position.y, 0), Quaternion.Euler(0, 0, randRotation));
            orangeBomb.transform.parent = _projectileParent;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator WaitForDecision(float waitTime)
    {
        while (waitTime > 0)
        {
            yield return new WaitForSeconds(1f);
            waitTime--;
        }
        AttackDecicison();
    }
}
