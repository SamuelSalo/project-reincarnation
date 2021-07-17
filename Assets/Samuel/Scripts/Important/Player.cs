using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private PlayerControls playerControls;

    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float moveSmoothing;
    [HideInInspector] public float dashCooldown;
    [HideInInspector] public float dashSpeed;
    [HideInInspector] public float stamina;

    private float dashTimer;
    private float attackTimer;
    private bool dash;
    private bool recoveringStamina;
    private bool dashSlowdown;

    [HideInInspector] public Vector2 moveDirection = Vector2.zero;
    private Vector2 refVelocity = Vector2.zero;
    private Vector2 rawInput;

    private Character character;
    
    [HideInInspector] public bool freeze;
    [HideInInspector] public bool slowed;
    [HideInInspector] public float currentSpeed;
    private float attackDuration;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            //Bind inputactions to functions
            playerControls.Gameplay.Attack.performed += context => Attack();
            playerControls.Gameplay.Dash.performed += context => Dash();

            playerControls.Gameplay.Movement.performed += context => rawInput = context.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Start()
    {
        character = GetComponent<Character>();
        stamina = character.maxStamina;
        attackDuration = character.faction == Character.Faction.Blue ? 0.5f : 0.7f;
    }

    private void Update()
    {
        if (freeze) { character.rb.velocity = Vector2.zero; return; }

        UpdateStamina();

        if (!slowed)
        {
            currentSpeed = character.animator.GetCurrentAnimatorStateInfo(0).IsName("attack") ? .2f : moveSpeed;
            currentSpeed = dashSlowdown ? 1f : moveSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (freeze) return;
        moveDirection = rawInput.normalized;
        character.rb.velocity = Vector2.SmoothDamp(character.rb.velocity, moveDirection * currentSpeed, ref refVelocity, moveSmoothing);
        character.UpdateAnimator(rawInput);

        if (dash)
        {
            StartCoroutine(DashIFrames());
            character.rb.velocity = moveDirection * dashSpeed;
            dash = false;
            GameSFX.instance.PlayDashSFX();

            stamina -= 25f;
            StopCoroutine(nameof(DashRoutine));
            StartCoroutine(DashRoutine());
        }
    }

    /// <summary>
    /// Activates a temporary hurtbox and deals damage to all hostile characters inside it.
    /// </summary>
    public void ActivateAttackHurtbox()
    {
        GameSFX.instance.PlaySlashSFX();
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
        if(Time.time >= attackTimer && stamina > 25f)
        {
            attackTimer = Time.time + 1f / character.attackRate;
            stamina -= 25f;

            StopCoroutine(nameof(AttackRoutine));
            StartCoroutine(AttackRoutine());

            character.animator.SetTrigger("attack");
        }
    }

    /// <summary>
    /// Ques a new dash to happen on next FixedUpdate
    /// </summary>
    private void Dash()
    {
        if(Time.time >= dashTimer && stamina > 25f && rawInput != Vector2.zero)
        {
            dashTimer = Time.time + dashCooldown;
            dash = true;
        }
    }

    private IEnumerator AttackRoutine()
    {
        recoveringStamina = false;
        freeze = true;
        yield return new WaitForSeconds(attackDuration);
        recoveringStamina = true;
        freeze = false;
    }
    private IEnumerator DashRoutine()
    {
        recoveringStamina = false;
        dashSlowdown = true;
        yield return new WaitForSeconds(0.5f);
        recoveringStamina = true;
        dashSlowdown = false;
    }
    private IEnumerator DashIFrames()
    {
        character.invincible = true;
        yield return new WaitForSeconds(0.2f);
        character.invincible = false;
    }
    
    /*
    
    FOR LATER USE

    private Vector2 SnapVector2(Vector2 vectorToSnap)
    {
        Vector2 returnVector = new Vector2();

        returnVector.x = Mathf.RoundToInt(vectorToSnap.x);
        returnVector.y = Mathf.RoundToInt(vectorToSnap.y);

        return returnVector;
    }
    */
}