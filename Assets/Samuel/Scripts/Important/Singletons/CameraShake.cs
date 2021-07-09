using UnityEngine;
using System.Collections;

/// <summary>
/// Camera shake effect
/// </summary>
public class CameraShake : MonoBehaviour
{
    #region Singleton

    public static CameraShake instance;

    private void Awake()
    {
        if (instance)
        {
            Debug.Log("Multiple instances of " + name + " found!");
            return;
        }

        instance = this;
    }

    #endregion

    public Transform cameraHolder;

    public void Shake(float _duration, float _strength)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine(_duration, _strength));
    }

    private IEnumerator ShakeRoutine(float _duration, float _strength)
    {
        float maxStrength = _strength;
        float frameAmount = _duration * 60;
        
        //count frames and shake per frame
        while(frameAmount > 0)
        {
            yield return new WaitForFixedUpdate();
            frameAmount--;

            _strength = maxStrength * ((frameAmount / 60) / _duration);
            var shakeVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * _strength;

            cameraHolder.transform.position += (Vector3)shakeVector;
        }
    }
}