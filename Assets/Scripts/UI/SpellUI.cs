using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellUI : MonoBehaviour
{
    [SerializeField] SelectSpell selectSpell;
    [SerializeField] Image[] sprites;
    [SerializeField] Image[] cooldownSprites;
    [SerializeField] TMP_Text[] spellTexts;

    Spell[] spells = new Spell[5];
    float[] currentCooldowns = new float[5];

    private void Start()
    {
        UpdateSpellList(selectSpell.spells);
    }

    void Update()
    {
        currentCooldowns = selectSpell.spellCooldowns;
        for (int i = 0; i < spells.Length; i++)
        {
            if (spells[i] != null)
            {
                float cooldown = currentCooldowns[i];
                if (cooldown > 0)
                {
                    spellTexts[i].gameObject.SetActive(true);
                    string text = ((int)(cooldown * 10) / 10f).ToString();
                    spellTexts[i].text = text;
                    cooldownSprites[i].fillAmount = cooldown / spells[i].baseCooldown;
                }
                else
                {
                    spellTexts[i].gameObject.SetActive(false);
                    cooldownSprites[i].fillAmount = 0;
                }
            }
        }
    }

    public void UpdateSpellList(Spell[] spellList)
    {
        spells = spellList;
    }
}
