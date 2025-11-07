using UnityEngine;
using TMPro;

public class BoomExplosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    public GameObject explosionEffect;      // Assign explosion prefab
    public AudioClip explosionSound;        // Assign explosion sound
    public float destroyDelay = 1f;         // Wait before destroying after explosion
    public TextMeshPro ScoreText1;
    public TextMeshPro ScoreText2;
    public timer Timer;

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
        if (collision.gameObject.CompareTag("Bird") && Timer.player1 == true)
        {
            int currentScore = int.Parse(ScoreText1.text);
            currentScore -= 5;
            ScoreText1.text = currentScore.ToString();
        }
        else if (collision.gameObject.CompareTag("Bird") && Timer.player1 == false)
        {
            int currentScore = int.Parse(ScoreText2.text);
            currentScore -= 5;
            ScoreText2.text = currentScore.ToString();
        }
        if (collision.gameObject.CompareTag("Player2"))
        {
            int currentScore = int.Parse(ScoreText1.text);
            currentScore += 10;
            ScoreText1.text = currentScore.ToString();
        }
        else if (collision.gameObject.CompareTag("Player1"))
        {
            int currentScore = int.Parse(ScoreText2.text);
            currentScore += 10;
            ScoreText2.text = currentScore.ToString();
        }

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
