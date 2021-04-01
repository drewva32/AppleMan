using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEndEventHelper : MonoBehaviour
{
    private IHaveAnimationEndEvent _parentObject;

    private void Awake()
    {
        _parentObject = GetComponentInParent<IHaveAnimationEndEvent>();
    }

    public void OnAnimationEnd()
    {
        _parentObject.OnAnimationEnd();

    }
}
