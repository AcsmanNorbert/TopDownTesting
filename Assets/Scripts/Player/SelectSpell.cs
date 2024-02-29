using UnityEngine;
using static Spell;

public class SelectSpell : MonoBehaviour
{
    [SerializeField] GameObject mesh;
    [SerializeField] Transform[] shootingTransforms;

    public Spell[] spells = new Spell[5];
    public float[] spellCooldowns { private set; get; } = new float[5];

    private void Update()
    {
        if (GameManager.isPaused) return;

        if (Input.GetKey(KeyCode.Mouse0))
            CastSpell(0);
        if (Input.GetKeyDown(KeyCode.Q))
            CastSpell(1);
        if (Input.GetKeyDown(KeyCode.E))
            CastSpell(2);
        if (Input.GetKeyDown(KeyCode.R))
            CastSpell(3);
        if (Input.GetKeyDown(KeyCode.F))
            CastSpell(4);

        int index = 0;
        foreach (var item in spells)
        {
            if (item != null)
                if (spellCooldowns[index] > 0)
                    spellCooldowns[index] -= Time.deltaTime;
                else if (spellCooldowns[index] < 0)
                    spellCooldowns[index] = 0;
            index++;
        }
    }

    private void CastSpell(int index)
    {
        Spell currentSpell = spells[index];
        if (currentSpell != null)
        {
            if (spellCooldowns[index] <= 0)
            {
                GameObject spell = Instantiate(spells[index].prefab);
                spellCooldowns[index] = spells[index].baseCooldown;

                switch (currentSpell.castType)
                {
                    case CastType.Projectile:
                        Transform shootingTransform = shootingTransforms[currentSpell.castPosition];
                        spell.transform.position = shootingTransform.position;
                        spell.transform.rotation = shootingTransform.rotation;
                        break;
                    case CastType.Cursor:
                        Vector3 shootingPosition = PlayerInput.GetMousePosition(mesh.transform);
                        shootingPosition.y = 0;
                        spell.transform.position = shootingPosition;
                        if(currentSpell.castPosition == 1)
                            spell.transform.rotation = mesh.transform.rotation;
                        break;
                    case CastType.Melee:
                        spell.transform.position = mesh.transform.position;
                        if (currentSpell.castPosition == 0)
                            spell.transform.SetParent(mesh.transform);
                        break;
                }
            }
        }
    }
}
