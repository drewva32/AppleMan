using System;
using UnityEngine;

public class StateTransition
{
    public  PlayerState From { get; private set; }
    public  PlayerState To { get; private set; }
    public  Func<bool> Condition { get; private set; }

    public StateTransition(PlayerState from, PlayerState to, Func<bool> condition)
    {
        From = @from;
        To = to;
        Condition = condition;
    }
}