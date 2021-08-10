using System.Collections;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    #region Singleton

    public static Tooltip instance;

    private void Awake()
    {
        if(instance)
        {
            Debug.LogWarning("Multiple tooltip instances detected.");
            return;
        }

        instance = this;
    }

    #endregion
    private TMP_Text tooltipText;

    private void Start()
    {
        tooltipText = GetComponent<TMP_Text>();
    }

    private void OnDisable()
    {
        StopCoroutine(nameof(TooltipRoutine));
        tooltipText.enabled = false;
    }

    public void ShowTooltip(string _tooltipText, float _time)
    {
        StopCoroutine(nameof(TooltipRoutine));
        tooltipText.enabled = false;
        StartCoroutine(TooltipRoutine(_tooltipText, _time));
    }

    private IEnumerator TooltipRoutine(string _text, float _time)
    {
        tooltipText.enabled = true;
        tooltipText.text = _text;

        yield return new WaitForSecondsRealtime(_time);

        tooltipText.enabled = false;
    }
}
