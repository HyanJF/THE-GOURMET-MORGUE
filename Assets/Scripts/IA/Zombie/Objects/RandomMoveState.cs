using UnityEngine;

[CreateAssetMenu(fileName = "RandomMoveState", menuName = "FSM/States/RandomMove")]
public class RandomMoveState : State
{
    public float speed = 2f;
    public float changeDirectionTime = 2f;

    private Vector3 currentDirection;
    private float timer;

    public override void EnterState(StateMachine stateMachine)
    {
        ChangeDirection();
        timer = 0f;
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        timer += Time.deltaTime;
        if (timer >= changeDirectionTime) 
        {
            ChangeDirection();
            timer = 0f;
        }

        stateMachine.transform.Translate(currentDirection * speed * Time.deltaTime, Space.World);
    }

    private void ChangeDirection()
    {
        currentDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }
}
