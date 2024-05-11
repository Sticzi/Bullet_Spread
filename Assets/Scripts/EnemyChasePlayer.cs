using UnityEngine;

public class EnemyChasePlayer : MonoBehaviour
{
    public float speed = 5.0f;
    public float collisionCheckRadius = 1.0f; // Radius for checking proximity to other enemies    

    private Transform playerTransform;
    private Rigidbody2D rb;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, collisionCheckRadius);

        
        foreach (var hit in hits)
        {
            if (hit.gameObject.CompareTag("Enemy") && hit.gameObject != gameObject) // Replace "EnemyTag" with your actual enemy tag
            {
                //move the enemy away from other enemies
                Vector2 collisionDirection = transform.position - hit.transform.position;                     
                transform.position += (Vector3)collisionDirection.normalized * Time.deltaTime * speed/2;

            }            
        }

        //move the enemy towards the player
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;        
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, collisionCheckRadius);
    }
}
