using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [Range(1, 4)] public float moveSpeed = 3f;
    [Range(0.05f, 0.3f)] public float moveSmoothing = 0.1f;
    [Range(0.05f, 1f)] public float turnSmoothing = 0.5f;

    [Space] [Header("Dashing")]
    [Range(0f, 3f)] public float dashCooldown = 1f;
    [Range(10f, 20f)] public float dashSpeed = 15f;

    [HideInInspector] public float stamina;
    private float dashTimer;
    private float attackTimer;
    private bool dash;
    private bool recoveringStamina;

    [HideInInspector] public Vector2 moveDirection = Vector2.zero;
    private Vector2 lookDirection;
    private Vector2 refVelocity = Vector2.zero;

    private Rigidbody2D rb;
    private Character character;
    private GameSFX gameSFX;
    [HideInInspector] public bool teleporting;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
        gameSFX = GameObject.FindWithTag("AudioManager").GetComponent<GameSFX>();
        stamina = character.maxStamina;
    }
    private void Update()
    {
        if (teleporting) { rb.velocity = Vector2.zero; return; }

        GetInput();
        UpdateStamina();
    }

    // Move smoothly according to movement vector in relation to player direction.
    // Rotate character towards mouse cursor.
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

            stamina -= 20f;
            StopCoroutine(nameof(RecoveryDelay));
            StartCoroutine(RecoveryDelay());

        }
    }

    /// <summary>
    /// Activates a temporary hurtbox and deals damage to all hostile characters inside it.
    /// </summary>
    public void ActivateAttackHurtbox()
    {
        gameSFX.PlaySlashSFX();
        var hits = Physics2D.OverlapBoxAll(transform.position + transform.up, new Vector2(1f, 1f), 0f);
        if (hits.Length != 0)
        {
            foreach (Collider2D hit in hits)
            {
                if (hit.transform.CompareTag("AI") && hit.transform.GetComponent<Character>().faction != character.faction)
                    character.DealDamage(character.damage, hit.transform.GetComponent<Character>());
            }
        }
    }

    /// <summary>
    /// Get player inputs
    /// </summary>
    private void GetInput()
    {
        //Movement Input
        moveDirection = transform.TransformDirection(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var heading = mousePos - transform.position;
        lookDirection = heading / heading.magnitude;
        moveDirection.Normalize();

        //Attack input
        if (Input.GetKeyDown(KeyCode.F) && Time.time >= attackTimer && stamina > 20f)
        {
            attackTimer = Time.time + 1f / character.attackRate;
            stamina -= 20f;

            StopCoroutine(nameof(RecoveryDelay));
            StartCoroutine(RecoveryDelay());

            character.animator.SetTrigger("Attack");
        }
        //Dash input
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= dashTimer && stamina > 20f)
        {
            dashTimer = Time.time + dashCooldown;
            dash = true;
        }
    }

    /// <summary>
    /// Update stamina UI and variables
    /// </summary>
    private void UpdateStamina()
    {
        //Stamina recovery
        if (recoveringStamina)
        {
            stamina += Time.deltaTime * character.staminaRecovery;
            stamina = Mathf.Clamp(stamina, 0f, character.maxStamina);
        }
    }

    private IEnumerator RecoveryDelay()
    {
        recoveringStamina = false;
        yield return new WaitForSeconds(0.5f);
        recoveringStamina = true;
    }
}