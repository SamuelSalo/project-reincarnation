using UnityEngine;

public static class Utils
{
    public static bool PercentageChance(float _percentage)
    {
        return Random.Range(0, 100) <= _percentage;
    }

    public static float Map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
