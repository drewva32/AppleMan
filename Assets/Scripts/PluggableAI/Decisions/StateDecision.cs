using UnityEngine;

public abstract class StateDecision : ScriptableObject
{
    public abstract bool Condition(PluggableStateController controller);
}
