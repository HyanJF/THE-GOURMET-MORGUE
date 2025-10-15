using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "FollowPlayerState", menuName = "FSM/States/FollowPlayer")]
public class FollowPlayerState : State
{
    public float stopDistance = 2f;
    public float lostSightTime = 5f;

    private float timer = 0f;

    public override void EnterState(StateMachine stateMachine)
    {
        if (stateMachine.context.detectionSphere != null)
            stateMachine.context.detectionSphere.enabled = true;
        timer = 0f;
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        var agent = stateMachine.context.agent;
        var player = stateMachine.context.player;

        if (agent == null || player == null) return;

        float distance = Vector3.Distance(stateMachine.transform.position, player.transform.position);

        if (distance > stopDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.isStopped = true;
        }

        if (distance > stateMachine.context.detectionSphere.radius * 2f)
        {
            timer += Time.deltaTime;
            if (timer >= lostSightTime)
            {
                foreach (var t in transitions)
                {
                    if (t.state != null)
                    {
                        stateMachine.ChangeState(t.state);
                        return;
                    }
                }
            }
        }
        else
        {
            timer = 0f;
        }
    }

    public override void ExitState(StateMachine stateMachine)
    {
        if (stateMachine.context.detectionSphere != null)
            stateMachine.context.detectionSphere.enabled = false;
    }
}
