using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFlash: MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool flashing;

    /// <summary>
    /// Call coroutine to flash attatched sprite.
    /// </summary>
    public void Flash()
    {
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();

        if(flashing)
            StopCoroutine(FlashCoroutine());

        StartCoroutine(FlashCoroutine());
    }

    /// <summary>
    /// Flash attached sprite renderer for amt of seconds.
    /// </summary>
    private IEnumerator FlashCoroutine()
    {
        flashing = true;
        spriteRenderer.material.SetFloat("_Flash", 20f);

        yield return new WaitForSeconds(0.05f);

        flashing = false;
        spriteRenderer.material.SetFloat("_Flash", 1f);
    }
}
