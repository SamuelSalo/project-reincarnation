using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Range(0.001f, 0.02f)]
    public float smoothing;

    public Transform target;

    // Smoothly follow target character.
    private void LateUpdate()
    {
        if (!target) return;

        var targetPosition = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
    }
}
