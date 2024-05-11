using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointsSystem : MonoBehaviour
{
    public static PointsSystem Instance { get; private set; }

    public int currentPoints;
    public int pointsToLevelUp = 500;
    public int currentLevel = 1;

    public Slider pointsBar;
    public TMP_Text levelText;
    
    public GameObject levelUpMenu; // Referencja do menu poziomu
    private GameObject player;

    private void Awake()
    {
        Instance = this;
    }    

    public void SetUIReferences(Slider slider, TMP_Text text, GameObject menu, GameObject Player)
    {
        player = Player;
        pointsBar = slider;
        levelText = text;
        //levelUpMenu = menu;
    }

    public void AddPoints(int points)
    {
        currentPoints += points;
        if (currentPoints >= pointsToLevelUp)
        {
            LevelUp();
        }

        UpdatePointsUI();
    }

    void UpdatePointsUI()
    {
        if (pointsBar != null)
        {
            pointsBar.value = (float)currentPoints / pointsToLevelUp;
        }
    }

    void LevelUp()
    {
        player.GetComponent<PlayerShooting>().playerLevelUp();
        currentPoints -= pointsToLevelUp;
        currentLevel++;
        UpdateLevelUI();
        //PauseGame();
    }

    void UpdateLevelUI()
    {
        if (levelText != null)
        {
            levelText.text = "Level: " + currentLevel.ToString();
            pointsToLevelUp = 500 * currentLevel; 
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0; // Zatrzymuje czas w grze
        levelUpMenu.SetActive(true); // Pokazuje menu poziomu
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // Wznawia czas w grze
        levelUpMenu.SetActive(false); // Ukrywa menu poziomu
    }
}
