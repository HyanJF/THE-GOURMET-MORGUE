using UnityEngine;

[CreateAssetMenu(menuName = "SocialFSM/States/Ignorar")]
public class IgnorarState : SocialState
{
    public override void EnterState(SocialFSM fsm)
    {
        Debug.Log("Oye por que no me miras. O.o");
    }

    public override void ExitState(SocialFSM fsm)
    {
        Debug.Log("De verdad me estas ignorando");
    }
}
