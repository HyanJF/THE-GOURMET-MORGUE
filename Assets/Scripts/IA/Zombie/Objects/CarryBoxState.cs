using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[CreateAssetMenu(fileName = "CarryBoxState", menuName = "FSM/States/CarryBox")]
public class CarryBoxState : State
{
    public float pickUpDistance = 2f;
    public float deliveryDistance = 1.5f;
    public float searchRadius = 10f;

    private int currentTargetIndex = 0;
    private bool hasBox = false;
    private InteractableBox currentBox;
    private bool isDelivering = false;

    public override void EnterState(StateMachine stateMachine)
    {
        hasBox = false;
        currentBox = null;
        isDelivering = false;
        FindClosestBox(stateMachine);
        currentTargetIndex = 0;
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        if (isDelivering) return;
        var agent = stateMachine.context.agent;
        if (agent == null) return;

        if (!hasBox)
        {
            if (currentBox == null)
            {
                FindClosestBox(stateMachine);
                return;
            }

            agent.isStopped = false;
            agent.SetDestination(currentBox.transform.position);

            float distance = Vector3.Distance(stateMachine.transform.position, currentBox.transform.position);
            if (distance <= pickUpDistance)
            {
                PickUpBox(stateMachine);
            }
        }
        else
        {
            Transform target = stateMachine.context.boxTargets[currentTargetIndex];
            if (target == null) return;

            if (!agent.pathPending)
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
            }

            if (!agent.pathPending && agent.remainingDistance <= deliveryDistance)
            {
                agent.isStopped = true;
                stateMachine.StartCoroutine(DeliverAndSwitch(stateMachine));
            }
        }
    }

    private IEnumerator DeliverAndSwitch(StateMachine stateMachine)
    {
        isDelivering = true;
        DropBox(stateMachine);
        yield return new WaitForSeconds(2f);

        stateMachine.ChangeState(stateMachine.initialState);
    }

    private void FindClosestBox(StateMachine stateMachine)
    {
        Collider[] boxes = Physics.OverlapSphere(stateMachine.transform.position, searchRadius);
        float closestDist = Mathf.Infinity;
        InteractableBox closestBox = null;

        foreach (var col in boxes)
        {
            if (col.CompareTag("Box"))
            {
                float dist = Vector3.Distance(stateMachine.transform.position, col.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestBox = col.GetComponent<InteractableBox>();
                }
            }
        }

        currentBox = closestBox;
    }

    private void PickUpBox(StateMachine stateMachine)
    {
        if (currentBox == null) return;

        currentBox.Interact();

        currentBox.transform.SetParent(stateMachine.transform);
        currentBox.transform.localPosition = new Vector3(0, 1.2f, 1f);

        hasBox = true;
    }

    private void DropBox(StateMachine stateMachine)
    {
        if (currentBox == null) return;

        currentBox.Interact(); 
        currentBox.transform.SetParent(null);

        Vector3 dropPos = stateMachine.transform.position + stateMachine.transform.forward * 0.8f;
        dropPos.y = stateMachine.transform.position.y;
        currentBox.transform.position = dropPos;

        hasBox = false;
        currentBox = null;
    }
}
