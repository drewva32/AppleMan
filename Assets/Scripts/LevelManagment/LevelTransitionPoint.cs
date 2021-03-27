using UnityEngine;

public class LevelTransitionPoint : MonoBehaviour
{
    [SerializeField] private bool isLevelEndPoint;
    private GameLevel _gameLevel;

    private void Awake()
    {
        _gameLevel = GetComponentInParent<GameLevel>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player)
        {
            _gameLevel.LevelTransition(isLevelEndPoint);
            //transition screen
            //load next level
            //move character
            //fade back to level
        }
    }
}