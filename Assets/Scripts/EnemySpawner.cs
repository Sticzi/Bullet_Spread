using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate = 2.0f;
    private float nextSpawnTime;
    private Camera mainCamera;
    public float offCameraOffset;
    private float additionalHealth = 0f; // Additional health to apply to spawned enemies

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        additionalHealth = Mathf.FloorToInt(Time.time / 60.0f) * 10f; // Increase health by 5 every minute

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPosition = GetRandomOffScreenPosition();
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Apply the additional health to the spawned enemy
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.health += additionalHealth;
        }
    }

    Vector2 GetRandomOffScreenPosition()
    {
        float x, y;
        if (Random.value > 0.5f)
        {
            // Spawn off the left or right
            x = Random.value > 0.5f ? mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x - offCameraOffset : mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x + offCameraOffset;
            y = Random.Range(mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).y, mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y);
        }
        else
        {
            // Spawn off the top or bottom
            x = Random.Range(mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x, mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x);
            y = Random.value > 0.5f ? mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).y - offCameraOffset : mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y + offCameraOffset;
        }

        return new Vector2(x, y);
    }
}
