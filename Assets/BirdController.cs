using UnityEngine;

public class BirdController : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;

    [Header("Vertical Speed Settings")]
    public float minVerticalSpeed = 2f;
    public float maxVerticalSpeed = 5f;

    [Header("Lifetime")]
    public float lifeTime = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0f; // No falling

        // Random direction (Up or Down)
        float direction = Random.value < 0.5f ? -1f : 1f;

        // Random speed
        float speed = Random.Range(minVerticalSpeed, maxVerticalSpeed);

        // ✅ Only vertical velocity
        rb.velocity = new Vector2(0f, direction * speed);

        // ✅ Flip sprite if moving up/down (optional)
        // If your sprite should flip when moving down, change true/false below
        sr.flipY = direction < 0 ? false : true;

        // Auto destroy
        Invoke(nameof(Die), lifeTime);
    }

    public void Die()
    {
        BirdSpawner.instance.RespawnBird(2f); // Respawn delay
        Destroy(gameObject);
    }
}
