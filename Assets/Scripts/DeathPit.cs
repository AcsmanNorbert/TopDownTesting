using UnityEngine;

public class DeathPit : MonoBehaviour
{
    bool entered;

    private void OnTriggerEnter(Collider collider)
    {
        if (entered) return;
        if (collider.gameObject == GameManager.i.player)
        {
            entered = true;

            GameManager.i.SetGameState(GameManager.GameState.Dead);
        }
        else
            Destroy(collider.gameObject);
    }
}
