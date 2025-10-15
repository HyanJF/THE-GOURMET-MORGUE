using UnityEngine;

public abstract class State : ScriptableObject
{
    public Transition[] transitions;

    public virtual void EnterState(StateMachine stateMachine) { }
    public virtual void UpdateState(StateMachine stateMachine) { }
    public virtual void ExitState(StateMachine stateMachine) { }

    public void CheckTransitions(StateMachine stateMachine)
    {
        foreach (var transition in transitions)
        {
            if (transition.condition != null && transition.condition.Check(stateMachine))
            {
                stateMachine.ChangeState(transition.state);
                break;
            }
        }
    }
}
