using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthbar : MonoBehaviour
{
    public Image healthBarFill;
    
    [HideInInspector] public Transform target;
    [HideInInspector] public bool visible = true;
    private float visualValue, actualValue;
    public float visualSmoothing;

    private void Update()
    {
        if (!target) return;

        transform.position = (Vector2)target.position + new Vector2(0, 0.75f);
        transform.GetChild(0).gameObject.SetActive(visible);

        if(!Mathf.Approximately(visualValue, actualValue))
        {
            visualValue = Mathf.Lerp(visualValue, actualValue, Time.deltaTime * visualSmoothing);
            healthBarFill.fillAmount = visualValue;
        }
    }

    public void SetFillColor(Color _color)
    {
        healthBarFill.color = _color;
    }

    public void SetHealthValue(float _health, float _maxHealth)
    {
        //healthBarFill.fillAmount = _health / _maxHealth;
        actualValue = _health / _maxHealth;
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }
}
