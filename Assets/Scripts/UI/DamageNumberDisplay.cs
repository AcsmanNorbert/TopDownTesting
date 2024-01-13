using UnityEngine;
using TMPro;
using System.Collections;
using System.Threading;

public class DamageNumberDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private CanvasGroup transparency;
    [SerializeField] private bool animate = true;
    private Vector3 targetLocation;

    public void SetDisplay(Vector3 position, float damage, IDamageable.DMGType dmgType)
    {
        targetLocation = position;
        textBox.text = damage.ToString();
        switch (dmgType)
        {
            case IDamageable.DMGType.Direct:
                textBox.color = Color.white;
                textBox.horizontalAlignment = HorizontalAlignmentOptions.Center;
                break;
            case IDamageable.DMGType.AreaOfEffect:
                textBox.color = Color.cyan;
                textBox.horizontalAlignment = HorizontalAlignmentOptions.Right;
                break;
            case IDamageable.DMGType.DamageOverTime:
                textBox.color = Color.yellow;
                textBox.horizontalAlignment = HorizontalAlignmentOptions.Left;
                break;
        }
        StartCoroutine(Lerping());
    }

    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(targetLocation);
    }

    private IEnumerator Lerping()
    {
        if (animate)
        {
            float timer = 2f;
            while (timer > 0f)
            {
                targetLocation.y += 0.02f;
                yield return new WaitForSeconds(0.01f);
                timer -= 0.1f;
                transparency.alpha = timer;
            }
        }
        else
            yield return new WaitForSeconds(2f);
        GameObject.Destroy(gameObject);
    }
}
