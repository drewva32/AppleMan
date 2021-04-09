using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInteractions
{
    void TakePlayerHit(int damageAmount, Vector3 directionToPlayer, float amountOfForce);
}
