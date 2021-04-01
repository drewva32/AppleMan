using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    string AnimationName { get; }

    void OnEnter();
    void OnExit();
    void LogicUpdate();
    void FixedLogicUpdate();
    public void OnAnimationEnd();
    void TakeHit();
}
