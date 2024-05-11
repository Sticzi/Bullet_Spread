using UnityEngine;

public class RotateTowardsCursor : MonoBehaviour
{
    private Transform playerTransform;
    private Transform shotgunTransform;
    private bool isFlipped = false;

    public float angleOffset;

    private float upperAngle;
    private float lowerAngle;



    void Start()
    {
        playerTransform = transform.parent; // Assuming this script is still attached to the shotgun
        shotgunTransform = transform;
        upperAngle = 90 + angleOffset;
        lowerAngle = -90 - angleOffset;
    }

    void Update()
    {
        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        
        
        // Check if the player should be flipped based on the angle
        bool shouldFlip = angle > upperAngle || angle < lowerAngle;
        if (shouldFlip != isFlipped)
        {
            isFlipped = shouldFlip;
            playerTransform.localScale = new Vector3(-playerTransform.localScale.x, playerTransform.localScale.y, playerTransform.localScale.z);
            upperAngle = 90 + angleOffset;
            lowerAngle = -90 - angleOffset;
        }

        if (isFlipped)
        {
            // Adjust the angle for when the player is flipped
            angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            upperAngle = 90 - angleOffset;
            lowerAngle = -90 + angleOffset;
        }

        shotgunTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
