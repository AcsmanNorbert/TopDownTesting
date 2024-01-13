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

}
