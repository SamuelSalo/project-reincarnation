using UnityEngine;

public class TestKill : MonoBehaviour
{
    [SerializeField]
    public Character player1;
    [SerializeField]
    public Character player2;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
            player2.DealDamage(200f, player1);
    }
}
