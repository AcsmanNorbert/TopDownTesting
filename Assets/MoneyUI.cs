using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    void Start()
    {
        MoneyManager.OnMoneyChange += MoneyManager_OnMoneyChange;
        text.text = MoneyManager.playerMoney.ToString() + "$";
    }

    private void MoneyManager_OnMoneyChange()
    {
        text.text = MoneyManager.playerMoney.ToString() + "$";
    }

}
