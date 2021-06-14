using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/AIVariables", fileName = "New AIVariables")]
public class AIVariables : ScriptableObject
{
    public float chaseRange;
    public float patrolRange;
    public float attackRange;
    public float patrolSpeed;
}
