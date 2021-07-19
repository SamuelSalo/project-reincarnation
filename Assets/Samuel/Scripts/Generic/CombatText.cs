using UnityEngine;
using TMPro;
using System.Collections;

public class CombatText : MonoBehaviour
{
    #region Singleton

    public static CombatText instance;

    private void Awake()
    {
        if(instance)
        {
            Debug.LogWarning("Multiple CombatText instances found.");
            return;
        }

        instance = this;
    }

    #endregion

    public GameObject dmgTextPrefab;

    [Space]

    public Color lowDmgColor;
    public Color highDmgColor;

    [Space]

    public int lowDmgTreshold;
    public int highDmgTreshold;

    /// <summary>
    /// Automatic color combat text
    /// </summary>
    /// <param name="_damage"></param>
    /// <param name="_location"></param>
    public void ShowDamageText(float _damage, Vector2 _location)
    {
        var text = CreateCombatText(_location);

        text.text = _damage.ToString();
        float mappedValue = Utils.Map(_damage, lowDmgTreshold, highDmgTreshold, 0f, 1f);
        var color = Color.Lerp(lowDmgColor, highDmgColor, mappedValue);
        color.a = 1;
        text.color = color;
    }

    /// <summary>
    /// Overload with custom color
    /// </summary>
    /// <param name="_damage"></param>
    /// <param name="_location"></param>
    /// <param name=""></param>
    public void ShowDamageText(float _damage, Vector2 _location, Color32 _color)
    {
        var text = CreateCombatText(_location);
        text.text = _damage.ToString();
        text.color = _color;
    }

    private TMP_Text CreateCombatText(Vector2 _location)
    {
        var obj = Instantiate(dmgTextPrefab, _location, Quaternion.identity, transform);
        Destroy(obj, 0.4f);
        return obj.GetComponent<TMP_Text>();
    }
}