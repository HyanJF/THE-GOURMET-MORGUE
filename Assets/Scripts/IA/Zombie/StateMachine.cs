using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public class StateMachine : MonoBehaviour
{
    [Header("Estado Inicial")]
    public State initialState;
    public State currentState;

    public FSMContext context = new FSMContext();

    private void Start()
    {
        ChangeState(initialState);
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
            currentState.CheckTransitions(this);
        }
    }

    public void ChangeState(State newState)
    {
        if (newState == null || newState == currentState)
            return;

        if (currentState != null) 
            currentState.ExitState(this);

        currentState = newState;
        currentState.EnterState(this);
    }

    [System.Serializable]
    public class FSMContext
    {
        public GameObject player;
        public LayerMask layerInecesaria;
        public NavMeshAgent agent;
        public SphereCollider detectionSphere;
        public InteractableBox boxTarget;
        public Transform[] boxTargets;
    }
}
