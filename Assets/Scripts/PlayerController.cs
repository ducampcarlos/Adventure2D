using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public InputAction MoveAction;
    [SerializeField] private float speed = 5f;
    Rigidbody2D rb;
    Vector2 move;

    [Space]
    [Header("Lives")]
    [SerializeField] int maxHealth = 5;
    int currentHealth;
    public int health { get { return currentHealth; } }
    public int MaxHealth { get { return maxHealth; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
        //Debug.Log(move);
    }

    void FixedUpdate()
    {
        Vector2 position = (Vector2)rb.position + move * speed * Time.deltaTime;
        rb.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }
}
