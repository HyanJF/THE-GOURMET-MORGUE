using UnityEngine;

public abstract class SocialState : ScriptableObject
{
    public TransitionNPC[] transitions = new TransitionNPC[0];

    public virtual void EnterState(SocialFSM fsm) { }
    public virtual void UpdateState(SocialFSM fsm) { }

    public virtual void ExitState(SocialFSM fsm) { }

    public void CheckTransitions(SocialFSM fsm)
    {
        if (transitions == null) return;

        foreach (var transition in transitions)
        {
            if (transition == null) continue;
            if (transition.condition == null || transition.sState == null) continue;

            if (transition.condition.Check(fsm))
            {
                fsm.ChangeState(transition.sState);
                break;
            }
        }
    }
}
