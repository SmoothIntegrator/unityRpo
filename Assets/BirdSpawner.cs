using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public static BirdSpawner instance;
    public GameObject birdPrefab;

    void Awake()
    {
        instance = this;
    }

    public void RespawnBird(float delay)
    {
        Invoke(nameof(SpawnBird), delay);
    }

    void Start()
    {
        // Spawn first bird when match begins
        RespawnBird(2f);
    }

    void SpawnBird()
    {
        // Spawn somewhere vertically in screen center zone
        float y = Random.Range(-2f, 2f);
        Instantiate(birdPrefab, new Vector3(0f, y, 0f), Quaternion.identity);
    }
}