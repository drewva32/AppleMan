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
        float randAttack = Random.Range(1, 4);
        if (randAttack <= 1f)
            testAttack = "charge";
        else if (randAttack > 1f && randAttack <= 2f)
            testAttack = "focus";
        else if (randAttack > 2f && randAttack <= 3f)
            testAttack = "jump";
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
        float randX;
        float randRotation;

        randRotation = Random.Range(-135, -35);
        randX = Random.Range(leftBounds.position.x, rightBounds.position.x);
        GameObject orangeBomb = Instantiate(orangeRain, new Vector3(randX, rightBounds.position.y, 0), Quaternion.Euler(0, 0, randRotation));
        orangeBomb.transform.parent = _projectileParent;

        //for (int i = 0; i < randAmount; i++)
        //{
        //    randRotation = Random.Range(-135, -35);
        //    randX = Random.Range(leftBounds.position.x, rightBounds.position.x);
        //    GameObject orangeBomb = Instantiate(orangeRain, new Vector3(randX, rightBounds.position.y, 0), Quaternion.Euler(0, 0, randRotation));
        //    orangeBomb.transform.parent = _projectileParent;
        //}

        //StartCoroutine(DropOranges(randAmount));
    }

    public void ChargeAttack()
    {
        _wc.SetVelocity(Vector2.right * _wc.vectorDirection * _movespeed);
    }

    public void SlamAttack()
    {
        _wc.SetVelocity(new Vector2(0.5f * _wc.vectorDirection.x, _jumpPower));
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
        float timer = 0;
        while (timer < 1)
        {
            randRotation = Random.Range(-135, -35);
            randX = Random.Range(leftBounds.position.x, rightBounds.position.x);
            GameObject orangeBomb = Instantiate(orangeRain, new Vector3(randX, rightBounds.position.y, 0), Quaternion.Euler(0, 0, randRotation));
            orangeBomb.transform.parent = _projectileParent;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
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
