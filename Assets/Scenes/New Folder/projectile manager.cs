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
        currentProjectile = SpawnRandomProjectile(handPosition.position);
        standbyProjectile = SpawnRandomProjectile(standbyPosition.position);

        SetColliderEnabled(standbyProjectile, false);

        AttachSlingshot(currentProjectile);
    }

    void Update()
    {
        
        if (standbyProjectile != null)
            standbyProjectile.transform.position = standbyPosition.position;

        
        if (currentProjectile != null)
        {
            SlingshotDrag sling = currentProjectile.GetComponent<SlingshotDrag>();
            if (sling != null && !sling.hasLaunched)
            {
                currentProjectile.transform.position = handPosition.position;
            }
        }
    }

    public void OnProjectileDestroyed()
    {
        
        currentProjectile = standbyProjectile;

        SetColliderEnabled(currentProjectile, true);

        currentProjectile.transform.position = handPosition.position;

        AttachSlingshot(currentProjectile);

        standbyProjectile = SpawnRandomProjectile(standbyPosition.position);

        SetColliderEnabled(standbyProjectile, false);
    }

    GameObject SpawnRandomProjectile(Vector2 position)
    {
        float rand = Random.value;
        GameObject prefab = (rand <= 0.3f) ? rocketPrefab : bombPrefab;
        GameObject projectile = Instantiate(prefab, position, Quaternion.identity);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb == null) rb = projectile.AddComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
        if ((projectile.name.Contains("rocket") || projectile.name.Contains("Rocket")) && handPosition.name.Contains("Hand1"))
        {
            projectile.transform.Rotate(0f, 0f, -90f);
        } else if ((projectile.name.Contains("rocket") || projectile.name.Contains("Rocket")) && handPosition.name.Contains("Hand2"))
        {
            projectile.transform.Rotate(0f, 0f, 90f);
        }


        return projectile;
    }

    void AttachSlingshot(GameObject projectile)
    {
        SlingshotDrag sling = projectile.GetComponent<SlingshotDrag>();
        if (sling != null)
        {
            sling.projectileManager = this;
            sling.anchorPoint = handPosition;
        }
    }

    void SetColliderEnabled(GameObject obj, bool enabled)
    {
        if (obj == null) return;
        Collider2D col = obj.GetComponent<Collider2D>();
        if (col != null) col.enabled = enabled;
    }
}
