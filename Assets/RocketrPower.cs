using UnityEngine;

public class RocketPower : MonoBehaviour
{
    private SlingshotDrag slingshotDrag;
    private Rigidbody2D rb;
    private bool yFrozen = false;
    private bool hasBoosted = false;
    private float frozenY;

    void OnEnable()
    {
        // Get SlingshotDrag and Rigidbody2D references
        slingshotDrag = GetComponent<SlingshotDrag>();
        if (slingshotDrag != null)
            rb = slingshotDrag.rb;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>(); // fallback
    }

    void Update()
    {
        if (rb == null) return;

        // Only continue if rocket has been launched
        if (slingshotDrag != null && !slingshotDrag.hasLaunched)
            return;

        // Allow the boost only once
        if (!hasBoosted && Input.GetMouseButtonDown(1))
        {
            hasBoosted = true; // prevent future boosts

            // Get direction of current motion
            Vector2 dir = rb.velocity.normalized;
            if (dir == Vector2.zero)
                dir = Vector2.right; // fallback if velocity is zero

            // Apply impulse in direction of motion
            rb.AddForce(dir * 10f, ForceMode2D.Impulse);

            // Freeze Y position and disable gravity
            frozenY = rb.position.y;
            yFrozen = true;
            rb.gravityScale = 0f;

            // Optional: immediately zero out vertical velocity
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }

        // After Y is frozen, lock the rocket’s height
        if (yFrozen)
        {
            rb.position = new Vector2(rb.position.x, frozenY);
        }
    }
}