using System;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static float playerMoney;
    [SerializeField] float startingMoney;

    public static Action OnMoneyChange;

    private void Start()
    {
        if (startingMoney <= 0) return;
        playerMoney = startingMoney;
        OnMoneyChange?.Invoke();
    }

    public static void GainMoney(float money)
    {
        if (money == 0) return;
        playerMoney += money;
        Debug.Log(money);
        OnMoneyChange?.Invoke();
    }

    public static bool UseMoney(float money)
    {
        if (playerMoney < money) return false;
        playerMoney -= money;

        if (money == 0) return true;
        OnMoneyChange?.Invoke();
        return true;
    }
}
