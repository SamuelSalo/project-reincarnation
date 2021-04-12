using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Range(1, 4)] public float moveSpeed;
    [Range(0.05f, 0.3f)] public float moveSmoothing;
    [Range(0.05f, 0.3f)] public float turnSmoothing;
    
    [HideInInspector] public Vector2 moveDirection = Vector2.zero;
    private Vector2 refVelocity = Vector2.zero;

    private Rigidbody2D rb;
    private Character character;

    [HideInInspector] public bool rotationLock;
    [HideInInspector] public bool teleporting;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
    }

    // Get inputs to determine movement direction and rotation lock
    private void Update()
    {
        if (teleporting) return;

        rotationLock = Input.GetKey(KeyCode.LeftShift);
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveDirection.Normalize();
    }

    // Move smoothly according to movement vector.
    // Rotate character or strafe depending on rotation lock.
    private void FixedUpdate()
    {
        if (teleporting) return;

        rb.velocity = Vector2.SmoothDamp(rb.velocity, (moveDirection * moveSpeed), ref refVelocity, moveSmoothing);
        transform.up = rotationLock ? (Vector2)transform.up : Vector2.Lerp(transform.up, moveDirection, turnSmoothing);
        character.UpdateAnimator();
    }
}