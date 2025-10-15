using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SocialFSM/Conditions/InputEvent")]
public class InputEventCondition : SocialCondition
{
    public int boton;
    private bool triggered = false;

    public void OnInputAction(InputAction.CallbackContext context)
    {
        if (context.performed)
            triggered = true;
    }

    public override bool Check(SocialFSM fsm)
    {
        if (triggered)
        {
            triggered = false;
            return true;
        }
        return false;
    }
}
