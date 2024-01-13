using UnityEngine;
using TMPro;

public class DifficultyUI : MonoBehaviour
{
    public TMP_Text text;
    public BasicSpawner basicSpawner;

    void Update()
    {
        text.text = basicSpawner.difficultyRating.ToString();
        text.fontSize = 30f + basicSpawner.difficultyRating;
    }
}
