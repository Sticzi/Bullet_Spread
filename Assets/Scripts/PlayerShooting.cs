using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform barrelTransform;

    public float baseBulletSpeed = 10f;
    public float speedVariance = 1f;  // Variance in bullet speed
    public float spreadAngle = 5f;    // Maximum angle of deviation for spread
    public float shootingRate = 0.5f;
    private float shootingTimer;
    
    public int numberOfBullets = 3;   // Number of bullets to fire in each shot
   
    public bool useFixedSpread = false; // Toggle for fixed bullet spread


    void Update()
    {
        shootingTimer += Time.deltaTime;

        if (Input.GetButton("Fire1") && shootingTimer >= shootingRate)
        {
            Shoot();
            shootingTimer = 0;
        }
    }

    public void playerLevelUp()
    {
        numberOfBullets++;        // Increase the number of bullets by 1
        spreadAngle += 5f;        // Increase the spread angle by 5 degrees
    }


    void Shoot()
    {
        Vector2 shootingDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - barrelTransform.position);
        shootingDirection.Normalize();

        if (useFixedSpread)
        {
            // Apply fixed spread
            float startAngle = -spreadAngle / 2;
            float angleStep = spreadAngle / (numberOfBullets - 1);

            for (int i = 0; i < numberOfBullets; i++)
            {
                float angle = startAngle + angleStep * i;
                Vector2 modifiedDirection = Quaternion.Euler(0, 0, angle) * shootingDirection;
                FireBullet(modifiedDirection);
            }
        }
        else
        {
            // Variable spread
            for (int i = 0; i < numberOfBullets; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity);
                float speed = baseBulletSpeed + Random.Range(-speedVariance, speedVariance);

                float angleOffset = Random.Range(-spreadAngle, spreadAngle);
                Vector2 modifiedDirection = Quaternion.Euler(0, 0, angleOffset) * shootingDirection;

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.velocity = modifiedDirection * speed;
            }
        }
    }

    void FireBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * baseBulletSpeed;
        bullet.GetComponent<Bullet>().Initialize(direction);
    }
}
