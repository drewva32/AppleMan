using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Transition
{
    [SerializeField] private StateDecision _decision;
    [SerializeField] private State transitionToState;

    public StateDecision Decision => _decision;
    public State TransitionToState => transitionToState;
}
