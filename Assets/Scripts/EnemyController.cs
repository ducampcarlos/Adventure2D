using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float speed;
    Rigidbody2D rigidbody2d;
    [SerializeField] float changeTime = 3.0f;
    float timer;
    int direction = 1;

    [Header("Direction")]
    [SerializeField] bool vertical;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;

        if (vertical)
        {
            position.y = position.y + speed * direction * Time.deltaTime;
        }
        else
        {
            position.x = position.x + speed * direction * Time.deltaTime;
        }

        rigidbody2d.MovePosition(position);
    }
}
