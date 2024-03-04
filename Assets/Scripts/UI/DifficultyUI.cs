using UnityEngine;
using TMPro;
using System;

public class DifficultyUI : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    void Start()
    {
        RoomManager.OnDifficultyIncrease += RoomManager_OnDifficultyIncrease;
        text.text = 1.ToString();
    }

    private void RoomManager_OnDifficultyIncrease(object sender, EventArgs e)
    {
        text.text = (GameManager.i.roomManager.currentDifficulty).ToString();
    }
}
