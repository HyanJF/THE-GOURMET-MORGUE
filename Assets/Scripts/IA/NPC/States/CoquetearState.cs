using UnityEngine;

[CreateAssetMenu(menuName = "SocialFSM/States/Coquetear")]
public class CoquetearState : SocialState
{
    public override void EnterState(SocialFSM fsm)
    {
        Debug.Log("Oye por que tan coqueto 7w7");
    }

    public override void ExitState(SocialFSM fsm)
    {
        Debug.Log("Supongo que solo fue un reto TnT");
    }
}
