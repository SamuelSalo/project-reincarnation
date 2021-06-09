using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private PlayerControls playerControls;

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
    private Vector2 mousePosition;
    private Vector2 lookDirection;
    private Vector2 refVelocity = Vector2.zero;

    [HideInInspector] public Rigidbody2D rb;
    private Character character;
    private GameSFX gameSFX;
    [HideInInspector] public bool teleporting;
    private float currentSpeed;
    private bool attacking;
    private float attackDuration;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            //Bind inputactions to functions
            playerControls.Gameplay.Attack.performed += context => Attack();
            playerControls.Gameplay.Dash.performed += context => Dash();

            playerControls.Gameplay.Movement.performed += context => moveDirection = context.ReadValue<Vector2>();
            playerControls.Gameplay.MousePosition.performed += context => mousePosition = context.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
        gameSFX = GameObject.FindWithTag("AudioManager").GetComponent<GameSFX>();
        stamina = character.maxStamina;
        attackDuration = character.faction == Character.Faction.Blue ? 0.5f : 0.7f;
    }

    private void Update()
    {
        if (teleporting || attacking) { rb.velocity = Vector2.zero; return; }

        GetLookDirection();
        UpdateStamina();
        currentSpeed = character.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") ? .2f : moveSpeed;
    }

    private void FixedUpdate()
    {
        if (teleporting || attacking) return;
        
        rb.velocity = Vector2.SmoothDamp(rb.velocity, moveDirection * currentSpeed, ref refVelocity, moveSmoothing);
        transform.up = Vector2.Lerp(transform.up, lookDirection, turnSmoothing);

        character.UpdateAnimator();

        if (dash)
        {
            rb.velocity = moveDirection * dashSpeed;
            dash = false;
            gameSFX.PlayDashSFX();

            stamina -= 20f;
            StopCoroutine(nameof(DashRoutine));
            StartCoroutine(DashRoutine());
        }

        print("kb: " + moveDirection + ", m: " + mousePosition);
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
    /// Get look direction from mouse position
    /// </summary>
    private void GetLookDirection()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(mousePosition);
        var heading = mousePos - transform.position;
        lookDirection = heading / heading.magnitude;
        moveDirection.Normalize();
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

    /// <summary>
    /// Triggers attack functionality
    /// </summary>
    private void Attack()
    {
        if(Time.time >= attackTimer && stamina > 20f)
        {
            attackTimer = Time.time + 1f / character.attackRate;
            stamina -= 20f;

            StopCoroutine(nameof(AttackRoutine));
            StartCoroutine(AttackRoutine());

            character.animator.SetTrigger("Attack");
        }
    }

    /// <summary>
    /// Ques a new dash to happen on next FixedUpdate
    /// </summary>
    private void Dash()
    {
        if(Time.time >= dashTimer && stamina > 20f)
        {
            dashTimer = Time.time + dashCooldown;
            dash = true;
        }
    }

    private IEnumerator AttackRoutine()
    {
        recoveringStamina = false;
        attacking = true;
        yield return new WaitForSeconds(attackDuration);
        recoveringStamina = true;
        attacking = false;
    }
    private IEnumerator DashRoutine()
    {
        recoveringStamina = false;
        yield return new WaitForSeconds(0.5f);
        recoveringStamina = true;
    }
}