using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnInterval = 2.0f;
    public float spawnDistanceAhead = 50.0f;  // How far ahead of player to spawn obstacles
    public float spawnRangeWidth = 8.0f;      // Width of the road to randomize obstacle positions
    
    private float timer = 0.0f;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player not found! Make sure your player has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (playerTransform == null)
            return;
            
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0.0f;
        }
    }

    void SpawnObstacle()
    {
        // Calculate spawn position ahead of the player
        float spawnZ = playerTransform.position.z + spawnDistanceAhead;
        
        // Random X position within the road width
        float spawnX = Random.Range(-spawnRangeWidth / 2, spawnRangeWidth / 2);
        
        Vector3 spawnPosition = new Vector3(spawnX, 0, spawnZ);
        
        // Instantiate obstacle with random rotation for variety
        Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, rotation);
        
        // Destroy obstacles after they're behind the player (optional)
        Destroy(obstacle, 30.0f);
    }
}