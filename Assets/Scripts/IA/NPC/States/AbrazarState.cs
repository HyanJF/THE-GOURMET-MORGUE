using UnityEngine;

[CreateAssetMenu(menuName = "SocialFSM/States/Abrazar")]
public class AbrazarState : SocialState
{
    public override void EnterState(SocialFSM fsm)
    {
        Debug.Log("Uy por que tan cariñoso UwU");
    }

    public override void ExitState(SocialFSM fsm)
    {
        Debug.Log("Oye no espera no te vallas TnT");
    }
}
