using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SETUP
    public static GameManager i { get; private set; }

    void Start()
    {
        if (i == null)
            i = this;
    }
    #endregion

    public DamageNumberHandler numberDisplay;
    public GameObject player;

    public static bool isPaused { private set; get; } = false;

    public void PauseGame(bool doPause)
    {
        if (doPause)
        {
            Time.timeScale = 0;
            SetGameState(GameState.Pause);
        }
        else
        {
            Time.timeScale = 1;
            SetGameState(GameState.Playing);
        }
        isPaused = doPause;
    }

    public enum GameState
    {
        Playing,
        Dead,
        Pause
    }

    public static GameState currentGameState { get; set; } = GameState.Playing;

    public void SetGameState(GameState newState)
    {
        currentGameState = newState;
        if (newState == GameState.Dead)
            DeadState();
    }

    [SerializeField] GameObject deathMenuContainer;
    private void DeadState()
    {
        isPaused = true;
        Time.timeScale = 0f;
        deathMenuContainer.SetActive(true);
    }

}
