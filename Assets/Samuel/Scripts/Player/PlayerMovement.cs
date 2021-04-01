using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Range(1, 4)]
    public float moveSpeed;
    [Range(0.05f, 0.3f)]
    public float moveSmoothing;
    [Range(0.05f, 0.3f)]
    public float turnSmoothing;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 refVelocity = Vector2.zero;
    private Rigidbody2D rb;
    private bool rotationLock;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rotationLock = Input.GetKey(KeyCode.LeftShift);
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveDirection.Normalize();
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.SmoothDamp(rb.velocity, (moveDirection * moveSpeed), ref refVelocity, moveSmoothing);

        if (!rotationLock)
        {
            transform.up = Vector2.Lerp(transform.up, moveDirection, turnSmoothing);
        }

    }
}