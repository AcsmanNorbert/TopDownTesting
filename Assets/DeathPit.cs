using UnityEngine;

public class DeathPit : MonoBehaviour
{
    bool entered;
    private void OnCollisionEnter(Collision collision)
    {
        if (entered) return;
        if (collision.gameObject != GameManager.i.player) return;
 
        entered = true;

        GameManager.i.SetGameState(GameManager.GameState.Dead);
    }
}
