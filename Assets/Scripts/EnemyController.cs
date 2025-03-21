using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float speed;
    Rigidbody2D rigidbody2d;
    [SerializeField] float changeTime = 3.0f;
    float timer;
    int direction = 1;
    bool broken = true;

    [Header("Direction")]
    [SerializeField] bool vertical;

    Animator animator;

    [Header("Audio")]
    AudioSource audioSource;
    [SerializeField] AudioClip hitSound;

    [Header("Particles")]
    [SerializeField] ParticleSystem smokeEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
        if (!broken)
        {
            return;
        }
        Vector2 position = rigidbody2d.position;

        if (vertical)
        {
            position.y = position.y + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2d.MovePosition(position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }

    }

    public void Fix()
    {
        broken = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
        audioSource.Stop();
        PlaySound(hitSound);
        smokeEffect.Stop();
    }

    // Function to play an audio clip
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
