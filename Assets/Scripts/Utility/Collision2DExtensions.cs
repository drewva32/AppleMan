using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collision2DExtensions
{
    public static bool WasHitByPlayer(this Collision2D collision)
    {
        return collision.collider.GetComponent<Player>() != null;
    }

    public static bool WasHitfromTopSide(this Collision2D collision)
    {
        return collision.contacts[0].normal.y < -.95f;
    }
}
