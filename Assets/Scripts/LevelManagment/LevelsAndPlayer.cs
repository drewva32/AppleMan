using UnityEngine;

public class LevelsAndPlayer : MonoBehaviour
{
    public LevelManager LevelManager { get; private set; }
    public Player Player { get; private set; }
    private void Awake()
    {
         LevelManager = GetComponentInChildren<LevelManager>();
         Player = GetComponentInChildren<Player>();
    }
}
