using UnityEngine;
using TMPro;

public class BoomExplosion : MonoBehaviour

{

    [Header("Explosion Settings")]
    public SlingshotDrag slingshotDrag;
    public GameObject explosionEffect;
    public AudioClip explosionSound;
    public float destroyDelay = 1f;
    public TextMeshProUGUI ScoreText1;
    public TextMeshProUGUI ScoreText2;
    public timer Timer;
    public SlingshotDrag slingshotDragScript;

    private AudioSource audioSource;
    private bool hasExploded = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        if (ScoreText1 == null || ScoreText2 == null)
        {
            TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>(true);
            foreach (var text in texts)
            {
                if (text.name == "ScoreText1")
                    ScoreText1 = text;
                if (text.name == "ScoreText2")
                    ScoreText2 = text;
            }
        }

        if (Timer == null)
            Timer = FindObjectOfType<timer>();
        if (ScoreText1 == null) Debug.LogError("❌ ScoreText1 not found!");
        if (ScoreText2 == null) Debug.LogError("❌ ScoreText2 not found!");
        if (Timer == null) Debug.LogError("❌ Timer not found!");
    }
    void OnEnable()
    {
        slingshotDragScript = GetComponent<SlingshotDrag>();
        Collider2D myCollider = GetComponent<Collider2D>();
        if (myCollider == null) return;

        BoomExplosion[] allExplosions = FindObjectsOfType<BoomExplosion>();
        foreach (var other in allExplosions)
        {
            if (other == this) continue;
            Collider2D otherCollider = other.GetComponent<Collider2D>();
            if (otherCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, otherCollider, true);
            }
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasExploded) return;
    if (!slingshotDragScript.hasLaunched) return;
    if (collision.gameObject.GetComponent<BoomExplosion>() != null)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
            return;
        }

        if (collision.gameObject.CompareTag("MainCamera"))
            return;
        if (collision.gameObject.CompareTag("Bird") && Timer.player1 == true)
        {
            Destroy(collision.gameObject);
            int currentScore = int.Parse(ScoreText1.text);
            currentScore -= 5;
            ScoreText1.text = currentScore.ToString();
        }
        else if (collision.gameObject.CompareTag("Bird") && Timer.player1 == false)
        {
            Destroy(collision.gameObject);
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
        // Spawn explosion
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(effect, destroyDelay); // Let explosion animation play
        }

        // Play Sound
        if (explosionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        // Notify the ProjectileManager BEFORE destroying the projectile
        slingshotDragScript.projectileManager.OnProjectileDestroyed();

        // Destroy projectile immediately — explosion is separate so no delay
        Destroy(gameObject);
    }

}
