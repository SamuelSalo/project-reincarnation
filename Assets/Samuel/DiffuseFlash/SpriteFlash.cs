using UnityEngine;
using System.Collections;

public class SpriteFlash : MonoBehaviour
{
    /// <summary> Renderer who's material we want to cause to flash. Material must have a "_FlashAmount" float property ([0-1]) to tween. </summary>
    [SerializeField] private Renderer m_sprite;

    /// <summary> Rate at which the sprite will flash - specifies duration of one half of the interval, ie: "0.1" will spend 0.1 seconds on, then 0.1s off. </summary>
    public float FlashRate = 0.1f;

    /// <summary> Cache of the materials we're modifying. Cache is kept so they can be freed at the end of the component's lifespan. </summary>
    private Material[] m_spriteMaterialsCached;

    /// <summary> We also store the shared materials, so we can re-assign them for use on death. This keeps a reference to the non-instanced copy everyone else is using. </summary>
    private Material[] m_spriteSharedMaterialsCache;
    private IEnumerator m_flashDurationEnumerator;
    private bool m_isHighlighted;

    private void Awake()
    {
        // Get a copy of the SpriteRenderer's material and cache it so that when we flash,
        // we don't cause all things using that material to flash. We will clean this up
        // in OnDestroy.
        m_spriteSharedMaterialsCache = m_sprite.sharedMaterials;
        m_spriteMaterialsCached = m_sprite.materials;
    }

    public void Flash(int numTimes = 1, float delay = 0f)
    {
        if (m_flashDurationEnumerator != null)
            StopCoroutine(m_flashDurationEnumerator);

        m_flashDurationEnumerator = FlashInternal(numTimes, delay);
        StartCoroutine(m_flashDurationEnumerator);
    }

    public void StopFlashing()
    {
        StopCoroutine(m_flashDurationEnumerator);
        m_flashDurationEnumerator = null;

        // Ensure we're off, no matter the previous state of the flash.
        SetFlash(false);
    }

    private IEnumerator FlashInternal(int numTimes, float delay)
    {
        // Make sure we turn the flash off incase we're interrupting an old flash.
        SetFlash(false);

        yield return new WaitForSeconds(delay);

        // Iterate twice the length of times - that way numTimes is "how many times is it turned on", not "how many times does it flip".
        for (int i = 0; i < numTimes * 2; i++)
        {
            SetFlash(!m_isHighlighted);
            yield return new WaitForSeconds(FlashRate);
        }

        // Ensure we end on off state
        SetFlash(false);
    }


    private void SetFlash(bool makeSpriteWhite)
    {
        // Early out if we're asked to achieve the state we're already at
        // so we skip on setting the material property.
        if (m_isHighlighted == makeSpriteWhite || m_spriteMaterialsCached == null)
            return;

        for (int i = 0; i < m_spriteMaterialsCached.Length; i++)
            m_spriteMaterialsCached[i].SetFloat("_FlashAmount", makeSpriteWhite ? 1f : 0f);

        m_isHighlighted = makeSpriteWhite;
    }

    private void OnDestroy()
    {
        if (m_spriteMaterialsCached != null)
        {
            for (int i = 0; i < m_spriteMaterialsCached.Length; i++)
                Destroy(m_spriteMaterialsCached[i]);

            m_spriteMaterialsCached = null;

            // Tell the renderer to use the shared materials again so it doesn't disappear.
            m_sprite.sharedMaterials = m_spriteSharedMaterialsCache;
        }
    }
}
