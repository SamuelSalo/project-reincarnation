using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthbar : MonoBehaviour
{
    public Image healthBarFill;
    
    [HideInInspector] public Transform target;
    [HideInInspector] public bool visible = true;

    private void Update()
    {
        if (!target) return;

        transform.position = (Vector2)target.position + new Vector2(0, 1f);
        transform.GetChild(0).gameObject.SetActive(visible);
    }

    public void SetFillColor(Color _color)
    {
        healthBarFill.color = _color;
    }

    public void SetHealthValue(float _health, float _maxHealth)
    {
        healthBarFill.fillAmount = _health / _maxHealth;
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }
}
