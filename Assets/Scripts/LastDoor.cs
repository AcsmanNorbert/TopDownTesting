using TMPro;
using UnityEngine;

public class LastDoor : MonoBehaviour
{
    bool entered;
    [SerializeField] TMP_Text text;
    private void OnCollisionEnter(Collision collision)
    {
        if (entered) return;
        if (collision.gameObject != GameManager.i.player) return;
        entered = true;

        GameManager.i.SetGameState(GameManager.GameState.Dead);
        text.text = "You have won, good job!";
    }
}
