using UnityEngine;

public class ExperiencePoint : MonoBehaviour
{
    public float pickUpRadius = 2.0f; // Radius within which the gem is attracted to the player
    public float moveSpeed = 5.0f;    // Speed at which the gem moves towards the player
    public int experiencePointsValue = 25; // Points given by this gem
    private Transform player;
    private bool isAttracted = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isAttracted || Vector3.Distance(transform.position, player.position) < pickUpRadius)
        {
            isAttracted = true;
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }   

    private void OnTriggerEnter2D(Collider2D collision)
    {           
        if (collision.gameObject.CompareTag("Player"))
        {

            PointsSystem.Instance.AddPoints(experiencePointsValue);
            Destroy(gameObject); // Destroy the gem object
        }
    }

}
