using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public static BirdSpawner instance;
    public GameObject birdPrefab;
    public Transform[] spawnPoints; // assign in inspector

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SpawnBird();
    }

    public void SpawnBird()
    {
        // pick random spawn location
        int index = Random.Range(0, spawnPoints.Length);
        Instantiate(birdPrefab, spawnPoints[index].position, Quaternion.identity);
    }

    public void RespawnBird(float delay)
    {
        Invoke(nameof(SpawnBird), delay);
    }
}
