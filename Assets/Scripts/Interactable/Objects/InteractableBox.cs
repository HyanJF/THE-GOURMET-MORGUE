using UnityEngine;

public class InteractableBox : MonoBehaviour, IInteractable
{
    private bool isHeld = false;
    private Transform attachPoint;
    private Rigidbody rb;

    [Header("Colliders a desactivan al agarrar")]
    public Collider[] collidersToDisable;

    [Header("Detector de pared")]
    public SphereCollider wallTrigger;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        wallTrigger.enabled = false;
    }

    public void Interact()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;
        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        if (pm == null) return;

        if (!isHeld)
        {
            attachPoint = player.transform.Find("AttachPoint");
            if (attachPoint == null) return;

            foreach (var col in collidersToDisable)
                col.enabled = false; // desactivar colliders físicos

            transform.SetParent(attachPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            rb.isKinematic = true;

            if (wallTrigger != null)
                wallTrigger.enabled = true;

            isHeld = true;
        }
        else
        {
            transform.SetParent(null);
            rb.isKinematic = false;

            foreach (var col in collidersToDisable)
                col.enabled = true; // reactivar colliders físicos

            if (wallTrigger != null)
                wallTrigger.enabled = false;

            isHeld = false;
        }
    }

    public string GetPrompt()
    {
        return isHeld ? "Soltar caja" : "Agarrar caja";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isHeld) return;
        if (other.CompareTag("WallForBox"))
        {
            Interact(); // soltar al tocar pared invisible
        }
    }
}
