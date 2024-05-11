using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 2.0f; // Time in seconds before the bullet is automatically destroyed
    public float damage;
    private Vector2 bulletDirection;

    // Call this method to initialize the bullet's direction when firing
    public void Initialize(Vector2 direction)
    {
        bulletDirection = direction.normalized;
        // Optionally, set the bullet's velocity here if needed
    }

    void Start()
    {
        // Destroy the bullet after 'lifetime' seconds
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.GetHit(bulletDirection);
                // Do not destroy the bullet here. Let the EnemyHealth script handle it.
            }
        }
    }
}
