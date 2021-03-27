using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleBoy
{
    public class GameHud : Singleton<GameHud>
    {
        public UIHealth playerHealth;
        public UISpriteCounter extraLifeCounter;
        public UISpriteCounter coinCounter;

        [HideInInspector]
        public int coinCount;
        [HideInInspector]
        public int extraLifeCount;

        public void Start()
        {
            coinCount = 0;
            extraLifeCount = Constants.STARTING_EXTRA_LIVES;

            coinCounter.SetCount(coinCount);
            extraLifeCounter.SetCount(extraLifeCount);
        }

        public void AddCoins(int p_count)
        {
            coinCount += p_count;
            coinCounter.SetCount(coinCount);
        }

        public void AddLives(int p_count)
        {
            extraLifeCount += p_count;
            extraLifeCounter.SetCount(extraLifeCount);
        }
    }
}
