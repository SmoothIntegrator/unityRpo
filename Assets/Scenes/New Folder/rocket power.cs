using UnityEngine;

public class RocketPower : MonoBehaviour
{
    private SlingshotDrag slingshotDrag;

    void OnEnable()
    {
        slingshotDrag = GetComponent<SlingshotDrag>();
    }

    void Update()
    {
        // Make sure we have a valid reference
        if (slingshotDrag == null) return;

        // Check if the rocket has been launched
        if (slingshotDrag.hasLaunched)
        {
            // Right-click to apply horizontal impulse
            if (Input.GetMouseButtonDown(1))
            {
                Rigidbody2D rb = slingshotDrag.rb;
                if (rb != null)
                {
                    // Apply force only along the X-axis (horizontal)
                    rb.AddForce(Vector2.right * 10f, ForceMode2D.Impulse);
                }
            }
        }
    }
}


