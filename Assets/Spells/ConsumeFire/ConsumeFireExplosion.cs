using UnityEngine;
using UnityEngine.VFX;

public class ConsumeFireExplosion : MonoBehaviour
{
    [SerializeField] VisualEffect visualEffect;

    private void Start()
    {        
        ConsumeFireSpell.OnExplosion += ConsumeFireSpell_OnExplosion;
    }

    private void ConsumeFireSpell_OnExplosion(float range)
    {
        visualEffect.SetFloat("Size", range);
        visualEffect.Play();
    }
}
