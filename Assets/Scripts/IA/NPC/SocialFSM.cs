using UnityEngine;

public class SocialFSM : MonoBehaviour
{
    [Header("Estados")]
    public SocialState initialState;
    public SocialState saludarState;
    public SocialState coquetearState;
    public SocialState currentState;

    public FSMContext context = new FSMContext();

    private bool firstEntry = true;

    private void Start()
    {
        ChangeState(initialState);
    }
    public void ChangeState(SocialState newState)
    {
        if (newState == null || newState == currentState)
            return;

        if (currentState != null)
            currentState.ExitState(this);

        currentState = newState;
        currentState.EnterState(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (firstEntry)
            {
                ChangeState(saludarState);
                firstEntry = false;
            }
            else
            {
                ChangeState(coquetearState);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && currentState != null)
        {
            currentState.ExitState(this);
            currentState = initialState;
        }
    }

    [System.Serializable]
    public class FSMContext
    {
        public GameObject player;
        public Transform npc;
    }
}
