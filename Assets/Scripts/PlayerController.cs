using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public InputAction MoveAction;
    [SerializeField] private float speed = 5f;
    Rigidbody2D rb;
    Vector2 move;

    [Header("Actions")]
    public InputAction talkAction;
    public InputAction launchAction;

    [Space]
    [Header("Lives")]
    [SerializeField] int maxHealth = 5;
    int currentHealth;
    public int health { get { return currentHealth; } }
    public int MaxHealth { get { return maxHealth; } }

    // Variables related to temporary invincibility
    [SerializeField] float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;

    [Space]
    [Header("Projectile")]
    public GameObject projectilePrefab;

    [Space]
    [Header("Audio")]
    AudioSource audioSource;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip projectileSound;

    // Variables related to the player's animation
    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        talkAction.Enable();
        launchAction.Enable();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();

        }

        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        UpdateInvincible();

        if (launchAction.WasPressedThisFrame())
        {
            Launch();
        }
        
        if (talkAction.WasPressedThisFrame())
        {
            FindFriend();
        }
    }

    void FixedUpdate()
    {
        Vector2 position = (Vector2)rb.position + move * speed * Time.deltaTime;
        rb.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger("Hit");
            PlaySound(hitSound);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    void UpdateInvincible()
    {
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rb.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(moveDirection, 300);

        animator.SetTrigger("Launch");
        PlaySound(projectileSound);
    }

    void FindFriend()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position + Vector2.up * 0.1f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));
        if (hit.collider != null)
        {
            NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
            if (character != null)
            {
                UIHandler.instance.DisplayDialogue();
            }
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    
}
