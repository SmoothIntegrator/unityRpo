using UnityEngine;
using TMPro;

public class BoomExplosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    public GameObject explosionEffect;      // Assign explosion prefab
    public AudioClip explosionSound;        // Assign explosion sound
    public float destroyDelay = 1f;         // Wait before destroying after explosion
    public TextMeshPro ScoreText;

    private AudioSource audioSource;
    private bool hasExploded = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasExploded) return;

        // Ignore collision with camera
        if (collision.gameObject.CompareTag("MainCamera"))
            return;
        if (collision.gameObject.CompareTag("Building"))

            
        hasExploded = true;
        Explode();
    }

    void Explode()
    {
        // Spawn explosion effect
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(effect, destroyDelay); // destroy only the spawned effect
        }

        // Play sound
        if (explosionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        // Disable the bullet visuals and collider immediately
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        SpriteRenderer mr = GetComponent<SpriteRenderer>();
        if (mr != null) mr.enabled = false;

        // Destroy the bullet object after delay
        Destroy(gameObject, destroyDelay);
    }
}
