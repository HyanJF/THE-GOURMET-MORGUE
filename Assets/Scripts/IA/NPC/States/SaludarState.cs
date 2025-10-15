using UnityEngine;

[CreateAssetMenu(menuName = "SocialFSM/States/Saludar")]
public class SaludarState : SocialState
{
    public override void EnterState(SocialFSM fsm)
    {
        Debug.Log("Holi wis, bonito dia UwU");
    }

    public override void ExitState(SocialFSM fsm)
    {
        Debug.Log("O bueno adios Papu");
    }
}
