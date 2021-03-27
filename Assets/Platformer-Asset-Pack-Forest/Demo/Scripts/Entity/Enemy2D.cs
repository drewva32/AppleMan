using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleBoy
{
    public class Enemy2D : Entity2D
    {
        public virtual bool canJumpOnHead
        {
            get { return true; }
        }
    }
}
