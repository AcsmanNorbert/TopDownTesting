using TMPro;
using UnityEngine;

public class InteractButtonUI : MonoBehaviour
{
    [SerializeField] GameObject container;
    PlayerInteract playerInteract;
    [SerializeField] Animator animator;
    [SerializeField] TMP_Text moneyText;
    Transform targetTransform;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        playerInteract = GameManager.i.player.GetComponent<PlayerInteract>();
    }

    private void Update()
    {
        if(targetTransform != null)
            transform.position = Camera.main.WorldToScreenPoint(targetTransform.position);
        if (playerInteract.GetInteractObject() != null)
            Show(playerInteract.GetInteractObject());
        else
            Hide();                 
    }

    private void Show(IInteractable npcInteract)
    {
        targetTransform = npcInteract.GetTransform();
        float money = npcInteract.CostRequirement();
        moneyText.text = money.ToString()+"$";
        animator.SetBool("isActive", true);
    }

    private void Hide()
    {
        animator.SetBool("isActive", false);
    }
}
