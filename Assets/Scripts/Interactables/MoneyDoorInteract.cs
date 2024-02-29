using UnityEngine;

public class MoneyDoorInteract : MonoBehaviour, IInteractable
{
    [SerializeField] float moneyRequirement;
    [SerializeField] Animator animator;
    [SerializeField] GameObject[] destroyOthers;
    bool used;

    private void Start()
    {
        if(animator == null)
            animator = GetComponent<Animator>();
    }

    public void Interact(Transform interactorTransform)
    {
        if (used) return;
        if (MoneyManager.UseMoney(moneyRequirement))
        {
            used = true;
            foreach (var item in destroyOthers)
            {
                item.GetComponent<Animator>().SetTrigger("Open");
                Destroy(item.GetComponent<MoneyDoorInteract>());
            }
            animator.SetTrigger("Open");
            Destroy(this);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public float CostRequirement()
    {
        return moneyRequirement;
    }
}
