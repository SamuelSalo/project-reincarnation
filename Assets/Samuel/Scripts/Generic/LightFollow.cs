using UnityEngine;

public class LightFollow : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        if (!target) return;

        transform.position = target.position;
    }
}
