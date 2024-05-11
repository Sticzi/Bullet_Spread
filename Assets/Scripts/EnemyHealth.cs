using UnityEngine;
using DG.Tweening;

public class EnemyHealth : MonoBehaviour
{

    public float dropChance = 0.4f; // 40% chance to drop points    
    public float health = 20;
    public float hurtDuration;
    public float knockbackForce;
    public Sprite hurtSprite;
    private bool isHit = false;
    private Sprite defaultSprite;

    public GameObject experiencePointPrefab; // Assign this in the Inspector
    
    private SpriteRenderer spriteRenderer;
    private Collider2D enemyCollider;
    private Rigidbody2D rb;
    private Animator animator;




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
        animator = GetComponent<Animator>();
    }

    public void GetHit(Vector2 bulletDirection)
    {
        if (!isHit)
        {
            isHit = true;
            //int bulletHitCount = 0;

            // Find and destroy the first bullet within the enemy's collider area
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, enemyCollider.bounds.extents.magnitude);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Bullet"))
                {
                    TakeDamage(hit.GetComponent<Bullet>().damage, bulletDirection);
                    Destroy(hit.gameObject);

                    if(health<=0)
                    {
                        EnemyDeath();
                        break;
                    }                    
                }
            }            
        }

        isHit = false;
    }

    public void TakeDamage(float damage, Vector2 bulletDirection)
    {        
        health -= damage;
        

        //effects for taking damage 

        if (health > 0)
        {
            spriteRenderer.sprite = hurtSprite;
            Vector2 knockbackDirection = -bulletDirection.normalized; // Opposite direction of the bullet
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            spriteRenderer.sprite = hurtSprite;
            DOVirtual.DelayedCall(hurtDuration, () => spriteRenderer.sprite = defaultSprite);
        }         
    }



    void EnemyDeath()
    {
        // Additional logic for enemy death can be added here (e.g., animations, sound effects)
        animator.SetBool("Death", true);
        spriteRenderer.sprite = hurtSprite;
        DOVirtual.DelayedCall(hurtDuration, () => spriteRenderer.sprite = defaultSprite);

        // Drop experience points based on the drop chance
        if (Random.value <= dropChance)
        {
            Instantiate(experiencePointPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject, 0.45f); // Destroy the enemy
    }
}
