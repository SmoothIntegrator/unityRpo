using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [Header("Projectile Prefabs (2D)")]
    public GameObject bombPrefab;
    public GameObject rocketPrefab;

    [Header("Positions")]
    public Transform handPosition; 
    public Transform standbyPosition; 

    private GameObject currentProjectile;
    private GameObject standbyProjectile;

    void Start()
    {
        // Spawn initial projectiles
        currentProjectile = SpawnRandomProjectile(handPosition.position);
        standbyProjectile = SpawnRandomProjectile(standbyPosition.position);

        AttachSlingshot(currentProjectile);
    }

    public void OnProjectileDestroyed()
    {
        // Move standby into slingshot
        currentProjectile = standbyProjectile;

        // Smoothly move standby into hand (optional visual)
        currentProjectile.transform.position = Vector2.Lerp(
            currentProjectile.transform.position,
            handPosition.position,
            1f
        );

        AttachSlingshot(currentProjectile);

        // Spawn new standby
        standbyProjectile = SpawnRandomProjectile(standbyPosition.position);
    }

    GameObject SpawnRandomProjectile(Vector2 position)
    {
        float rand = Random.value;
        GameObject prefab = (rand <= 0.3f) ? rocketPrefab : bombPrefab;
        GameObject projectile = Instantiate(prefab, position, Quaternion.identity);

        // Make sure it has Rigidbody2D for physics
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb == null) rb = projectile.AddComponent<Rigidbody2D>();
        rb.gravityScale = 1f;

        return projectile;
    }

    void AttachSlingshot(GameObject projectile)
    {
        SlingshotDrag sling = projectile.GetComponent<SlingshotDrag>();
        if (sling != null)
            sling.anchorPoint = handPosition;
    }
}
//