using UnityEngine;

public class BirdController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [Header("Vertical Speed Settings")]
    public float minVerticalSpeed = 2f;
    public float maxVerticalSpeed = 4f;

    [Header("Lifetime")]
    public float autoDeathTime = 5f; // Bird lives max 5 sec if not hit

    private float minY;
    private float maxY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0f;

        // ✅ Lock X at center
        transform.position = new Vector3(0f, transform.position.y, 0f);

        // ✅ Calculate screen vertical limits
        Camera cam = Camera.main;
        float halfHeight = cam.orthographicSize;
        minY = -halfHeight + 1f;
        maxY = halfHeight - 1f;

        // ✅ Random direction (Up / Down)
        float direction = Random.value < 0.5f ? -1f : 1f;
        float speed = Random.Range(minVerticalSpeed, maxVerticalSpeed);

        // ✅ Apply only vertical movement
        rb.velocity = new Vector2(0f, direction * speed);

        // ✅ Optional sprite flip based on direction of movement
        sr.flipY = direction < 0 ? false : true;

        // ✅ Kill bird if not hit in 5 seconds
        Invoke(nameof(SelfDestruct), autoDeathTime);
    }

    void Update()
    {
        // ✅ Always stay at X = 0
        transform.position = new Vector3(0f, transform.position.y, 0f);

        // ✅ Bounce at top/bottom screen bounds
        if (transform.position.y <= minY && rb.velocity.y < 0)
            rb.velocity = new Vector2(0f, Mathf.Abs(rb.velocity.y));

        if (transform.position.y >= maxY && rb.velocity.y > 0)
            rb.velocity = new Vector2(0f, -Mathf.Abs(rb.velocity.y));
    }

    public void SelfDestruct()
    {
        // ✅ Schedule new bird after 10 seconds
        BirdSpawner.instance.RespawnBird(10f);

        Destroy(gameObject);
    }
}