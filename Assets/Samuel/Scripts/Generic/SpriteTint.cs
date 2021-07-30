using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteTint: MonoBehaviour
{
    public static Color DamageRed = new Color(1, 0, 0, 1);
    public static Color HealGreen = new Color(0, 1, 0, 1);
    public static Color SlowLBlue = new Color(0, 1, 1, 1);
    public static Color BloodRed = new Color(0.5f, 0, 0, 1);
    private SpriteRenderer spriteRenderer;
    private Color tintColor;

    private void Start()
    {
        tintColor.a = 0f;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// Flash sprite renderer with colour
    /// </summary>
    public void FlashColor(Color _color)
    {
        tintColor = _color;
        tintColor.a = 1f;
    }

    private void Update()
    {
        if(tintColor.a > 0f)
        {
            tintColor.a = Mathf.Clamp01(tintColor.a - 6f * Time.deltaTime);
            spriteRenderer.material.SetColor("_Tint", tintColor);
        }
    }
}
