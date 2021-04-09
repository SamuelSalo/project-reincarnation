using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightFlicker : MonoBehaviour
{
    public float randomness;
    private new Light2D light;
    private float radius;
    private float intensity;

    private void Start()
    {
        light = GetComponent<Light2D>();
        radius = light.pointLightOuterRadius;
        intensity = light.intensity;
    }

    private void FixedUpdate()
    {
        light.pointLightOuterRadius = Mathf.Lerp(light.pointLightOuterRadius, Random.Range(radius - randomness, radius + randomness), randomness);
        light.intensity = Mathf.Lerp(light.intensity, Random.Range(intensity - randomness/2, intensity + randomness/2), randomness);
    }
}
