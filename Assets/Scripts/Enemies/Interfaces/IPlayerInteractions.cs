using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInteractions
{
    void TakePunch(int damageAmount);
    void TakeSlide(int damageAmount, Vector3 directionToPlayer);
}
