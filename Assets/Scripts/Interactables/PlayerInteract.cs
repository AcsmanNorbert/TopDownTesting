using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] float interactRadius = 1f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            IInteractable interactable = GetInteractObject();
            if (interactable != null)
                interactable.Interact(transform);
        }
    }

    public IInteractable GetInteractObject()
    {
        List<IInteractable> npcInteractList = new List<IInteractable>();
        Collider[] rangeCheck = Physics.OverlapSphere(transform.position, interactRadius);
        foreach (Collider collider in rangeCheck)
        {
            IInteractable interactable = collider.GetComponent<IInteractable>();
            if (interactable == null) continue;
            if (collider.gameObject.activeSelf == false) continue;

            npcInteractList.Add(interactable);
        }

        IInteractable closestNPCInteract = null;
        foreach (IInteractable npcInteract in npcInteractList)
        {
            if (closestNPCInteract == null)
            {
                closestNPCInteract = npcInteract;
                continue;
            }
            if (Vector2.Distance(transform.position, npcInteract.GetTransform().position) <
                Vector3.Distance(transform.position, closestNPCInteract.GetTransform().position))
                closestNPCInteract = npcInteract;
        }

        return closestNPCInteract;
    }
}
