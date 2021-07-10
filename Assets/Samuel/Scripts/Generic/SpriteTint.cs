using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteTint: MonoBehaviour
{
    public static Color DamageRed = new Color(1, 0, 0, 1);
    public static Color HealGreen = new Color(0, 1, 0, 1);
    public static Color SlowLBlue = new Color(0, 1, 1, 0.66f);
    public static Color BloodRed = new Color(0.5f, 0, 0, 1);
    private SpriteRenderer spriteRenderer;
    private Color tintColor;
    private bool tinting;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// Flash sprite renderer with colour
    /// </summary>
    /// <param name="_color"></param>
    public void FlashColor(Color32 _color)
    {
        if (tinting) return;

        tintColor = _color;
    }

    /// <summary>
    /// Tint sprite renderer with color for a duration
    /// </summary>
    /// <param name="_color"></param>
    /// <param name="_duration"></param>
    public void DurationTint(Color32 _color, float _duration)
    {
        tintColor = _color;
        StartCoroutine(DurationTintRoutine(_duration));
    }

    private void Update()
    {
        if(spriteRenderer.color.a > 0f && !tinting)
        {
            tintColor.a = Mathf.Clamp01(tintColor.a - 6f * Time.deltaTime);
            spriteRenderer.material.SetColor("_Tint", tintColor);
        }
    }

    private IEnumerator DurationTintRoutine(float _duration)
    {
        tinting = true;
        spriteRenderer.material.SetColor("_Tint", tintColor);
        yield return new WaitForSeconds(_duration);
        tintColor.a = 0f;
        spriteRenderer.material.SetColor("_Tint", tintColor);
        tinting = false;
    }
}
