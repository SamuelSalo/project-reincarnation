using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [Range(1, 4)] public float moveSpeed;
    [Range(0.05f, 0.3f)] public float moveSmoothing;
    [Range(0.05f, 1f)] public float turnSmoothing;

    [Space] [Header("Dashing")]
    [Range(0f, 3f)] public float dashCooldown;
    [Range(10f, 20f)] public float dashSpeed;

    private float dashTimer;
    private bool dash;

    [HideInInspector] public Vector2 moveDirection = Vector2.zero;
    private Vector2 lookDirection;
    private Vector2 refVelocity = Vector2.zero;

    private Rigidbody2D rb;
    private Character character;
    private GameSFX gameSFX;
    [HideInInspector] public bool rotationLock;
    [HideInInspector] public bool teleporting;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
        gameSFX = GetComponent<GameSFX>();
    }

    // Get inputs to determine movement direction and rotation lock
    private void Update()
    {
        if (teleporting) { rb.velocity = Vector2.zero; return; }

        if(Input.GetKeyDown(KeyCode.Space) && Time.time >= dashTimer)
        {
            dashTimer = Time.time + dashCooldown;
            dash = true;
        }
        rotationLock = Input.GetKey(KeyCode.LeftShift);
        moveDirection = transform.TransformDirection(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"),0f);
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var heading = mousePos - transform.position;
        lookDirection = heading / heading.magnitude;
        moveDirection.Normalize();
    }

    // Move smoothly according to movement vector.
    // Rotate character or strafe depending on rotation lock.
    private void FixedUpdate()
    {
        if (teleporting) return;
        rb.velocity = Vector2.SmoothDamp(rb.velocity, moveDirection * moveSpeed, ref refVelocity, moveSmoothing);
        transform.up = Vector2.Lerp(transform.up, lookDirection, turnSmoothing);

        character.UpdateAnimator();

        if (dash)
        {
            rb.velocity = moveDirection * dashSpeed;
            dash = false;
            gameSFX.PlayDashSFX();
        }
        /* OLD MOVEMENT
        
        transform.up = rotationLock ? (Vector2)transform.up : Vector2.Lerp(transform.up, moveDirection, turnSmoothing);
        
        */
    }
}