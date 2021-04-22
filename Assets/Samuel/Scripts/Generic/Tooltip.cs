using System.Collections;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    private TMP_Text tooltipText;

    private void Start()
    {
        tooltipText = GetComponent<TMP_Text>();
    }
    public void ShowTooltip(string _tooltipText, float _time)
    {
        StopCoroutine(nameof(TooltipRoutine));
        StartCoroutine(TooltipRoutine(_tooltipText, _time));
    }

    private IEnumerator TooltipRoutine(string _text, float _time)
    {
        tooltipText.enabled = true;
        tooltipText.text = _text;

        yield return new WaitForSeconds(_time);

        tooltipText.enabled = false;
    }
}
