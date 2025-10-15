using UnityEngine;

[CreateAssetMenu(fileName = "TimeCondition", menuName = "FSM/Conditions/TimeCondition")]
public class TimeCondition : Condition
{
    public float timeToSwitch = 3f;
    private float timer = 0f;

    public override bool Check(StateMachine stateMachine)
    {
        timer += Time.deltaTime;
        if (timer >= timeToSwitch)
        {
            timer = 0f;
            return true;
        }
        return false;
    }
}
