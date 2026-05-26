using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerRunnerPro : MonoBehaviour
{
    [Header("Auto Run")]
    public float baseRunSpeed = 6f;
    public float maxRunSpeed = 14f;
    public float speedIncreasePerSecond = 0.15f;

    [Header("Jump")]
    public float jumpVelocity = 12f;
    public int maxJumps = 2;

    [Header("Tags")]
    public string groundTag = "Ground";
    public string obstacleTag = "Obstacle";

    [Header("Graze Slow")]
    [Range(0.1f, 1f)]
    public float grazeSlowMultiplier = 0.55f;
    public float grazeSlowDuration = 0.6f;

    [Header("Stuck Detection")]
    public float minMovePerSecond = 0.05f;
    public float stuckTimeToDie = 0.35f;

    [Header("Death + Respawn")]
    public Transform spawnPoint;
    public float respawnAfterSeconds = 1.2f;
    public float deathDropImpulse = 8f;

    public bool IsDead { get; private set; }

    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;

    private int jumpsLeft;
    private float currentRunSpeed;

    private float grazeTimer;

    private float lastX;
    private float stuckTimer;
    private bool touchingObstacle;

    private float respawnTimer;

    private GameManagerPro gm;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip deathClip;

    [Range(0f, 1f)]
    [SerializeField] private float jumpVolume = 0.7f;

    [Range(0f, 1f)]
    [SerializeField] private float deathVolume = 1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        gm = FindFirstObjectByType<GameManagerPro>();

        if (spawnPoint == null)
        {
            GameObject sp = new GameObject("SpawnPoint_Auto");
            sp.transform.position = transform.position;
            spawnPoint = sp.transform;
        }

        ResetState(true);
        lastX = transform.position.x;
    }

    void Update()
    {
        if (IsDead)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0f)
                Respawn();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpsLeft > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);

            // звук стрибка
            if (audioSource != null && jumpClip != null)
                audioSource.PlayOneShot(jumpClip, jumpVolume);

            jumpsLeft--;
        }
    }

    void FixedUpdate()
    {
        if (IsDead) return;

        currentRunSpeed = Mathf.Min(
            maxRunSpeed,
            currentRunSpeed + speedIncreasePerSecond * Time.fixedDeltaTime
        );

        float speed = currentRunSpeed;

        if (grazeTimer > 0f)
        {
            grazeTimer -= Time.fixedDeltaTime;
            speed *= grazeSlowMultiplier;
        }

        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

        // stuck detection
        float dx = Mathf.Abs(transform.position.x - lastX);
        lastX = transform.position.x;

        float movePerSecond = dx / Time.fixedDeltaTime;

        if (touchingObstacle && movePerSecond < minMovePerSecond)
        {
            stuckTimer += Time.fixedDeltaTime;
            if (stuckTimer >= stuckTimeToDie)
                Die();
        }
        else
        {
            stuckTimer = 0f;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsDead) return;

        if (collision.gameObject.CompareTag(groundTag))
        {
            jumpsLeft = maxJumps;
            return;
        }

        if (collision.gameObject.CompareTag(obstacleTag))
        {
            touchingObstacle = true;
            grazeTimer = grazeSlowDuration;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(obstacleTag))
            touchingObstacle = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(obstacleTag))
        {
            touchingObstacle = false;
            stuckTimer = 0f;
        }
    }

    // ----------------
    // DIE
    // ----------------
    void Die()
    {
        if (IsDead) return;

        IsDead = true;

        // звук смерті
        if (audioSource != null && deathClip != null)
        {
            audioSource.PlayOneShot(deathClip, deathVolume);
        }

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        if (anim != null)
            anim.enabled = false;

        col.enabled = false;
        rb.AddForce(Vector2.down * deathDropImpulse, ForceMode2D.Impulse);

        var cam = FindFirstObjectByType<CameraFollow>();
        if (cam != null)
        {
            cam.SnapToTarget();
            cam.frozen = true;
        }

        var manager = FindFirstObjectByType<GameManagerPro>();
        if (manager != null)
            manager.GameOver("Hit");
        else
            Debug.LogWarning("GameManagerPro not found in scene!");
    }



    // ----------------
    // RESPAWN
    // ----------------
    void Respawn()
    {
        transform.position = spawnPoint.position;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0;

        col.enabled = true;

        if (anim != null)
            anim.enabled = true;

        ResetState(true);

        lastX = transform.position.x;
    }

    void ResetState(bool resetSpeed)
    {
        IsDead = false;

        jumpsLeft = maxJumps;

        touchingObstacle = false;
        stuckTimer = 0f;

        grazeTimer = 0f;

        if (resetSpeed)
            currentRunSpeed = baseRunSpeed;
    }
}
