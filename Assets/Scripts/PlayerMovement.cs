using UnityEngine;
using UnityEngine.UI; // Required for UI elements
using DG.Tweening; // Import the DOTween namespace

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;
    public float invincibilityWearOffDelay = 0.5f;

    public bool enableIFrames = false;
    public SpriteRenderer spriteRenderer;

    public Color invincibleColor;
    public Color normalColor = Color.white;

    public int maxDashCharges = 3;
    public float dashChargeReloadTime = 2f;

    public Slider[] dashChargeSliders; // Array to hold the Sliders for the dash cooldowns

    private Vector2 moveInput;
    private bool isDashing;
    private int currentDashCharges;
    private float dashReloadTimer;

    private Tween invincibilityWearOffTween;

    void Start()
    {
        currentDashCharges = maxDashCharges;
        UpdateDashChargeUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && currentDashCharges > 0)
        {
            Dash();
        }

        MovePlayer();
        ReloadDashCharges();
    }

    void MovePlayer()
    {
        if (!isDashing)
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            Vector2 moveVelocity = moveInput * moveSpeed;
            transform.position += (Vector3)moveVelocity * Time.deltaTime;
        }
    }

    void Dash()
    {
        if (moveInput != Vector2.zero)
        {
            isDashing = true;
            currentDashCharges--; // Use one charge
            if (currentDashCharges == maxDashCharges - 1) // Start reload timer after first dash is used
            {
                dashReloadTimer = dashChargeReloadTime;
            }

            if (enableIFrames)
            {
                if (invincibilityWearOffTween != null)
                {
                    invincibilityWearOffTween.Kill(); // Cancel the previous wear-off timer if it exists
                }
                EnableInvincibility();
            }

            Vector3 dashTarget = transform.position + (Vector3)moveInput.normalized * dashDistance;
            transform.DOMove(dashTarget, dashDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                isDashing = false;
                // Reset or extend the invincibility wear-off timer
                invincibilityWearOffTween = DOVirtual.DelayedCall(invincibilityWearOffDelay, DisableInvincibility);
            });
        }
    }

    void ReloadDashCharges()
    {
        if (currentDashCharges < maxDashCharges && dashReloadTimer > 0)
        {
            dashReloadTimer -= Time.deltaTime;
            UpdateDashChargeUI(); // Update Sliders based on cooldown

            if (dashReloadTimer <= 0)
            {
                currentDashCharges++;
                if (currentDashCharges < maxDashCharges)
                {
                    dashReloadTimer = dashChargeReloadTime;
                }
                UpdateDashChargeUI(); // Update Sliders when a charge is reloaded
            }
        }
    }

    void UpdateDashChargeUI()
    {
        for (int i = 0; i < dashChargeSliders.Length; i++)
        {
            if (i < currentDashCharges)
            {
                dashChargeSliders[i].value = 1; // Charge available
                dashChargeSliders[i].gameObject.transform.Find("Fill Area/Fill").gameObject.SetActive(true); // Make fill visible
            }
            else if (i == currentDashCharges && currentDashCharges < maxDashCharges)
            {
                dashChargeSliders[i].value = 1 - (dashReloadTimer / dashChargeReloadTime); // Currently reloading
                                                                                           // Only make fill visible if value is greater than 0
                dashChargeSliders[i].gameObject.transform.Find("Fill Area/Fill").gameObject.SetActive(dashChargeSliders[i].value > 0);
            }
            else
            {
                dashChargeSliders[i].value = 0; // Charge used
                dashChargeSliders[i].gameObject.transform.Find("Fill Area/Fill").gameObject.SetActive(false); // Make fill non-visible
            }
        }
    }



    void EnableInvincibility()
    {
        spriteRenderer.color = invincibleColor;

        PlayerHealth health = GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.SetInvincibility(true);
        }
    }

    void DisableInvincibility()
    {
        spriteRenderer.color = normalColor;

        PlayerHealth health = GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.SetInvincibility(false);
        }
    }
}
