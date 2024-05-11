using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private bool isInvincible = false;

    public void SetInvincibility(bool value)
    {
        isInvincible = value;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && !isInvincible)
        {
            // Reset the game (this example reloads the current scene)
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
