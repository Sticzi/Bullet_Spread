using UnityEngine;
using TMPro; // Make sure to include this if you're using TextMeshPro for UI

public class LevelTimer : MonoBehaviour
{
    public TMP_Text timerText; // Reference to a TextMeshPro UI element to display the timer
    private float startTime;
    public int currentLevel;
    public float secondsPerLevel;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time; // Record the time at the start of the level        
    }

    // Update is called once per frame
    void Update()
    {
        float elapsedTime = Time.time - startTime; // Calculate the elapsed time since the start
        currentLevel = Mathf.FloorToInt(elapsedTime / secondsPerLevel); // Convert elapsed time to minutes

        // Update the timer text to display minutes and seconds
        string minutes = Mathf.Floor((elapsedTime % 3600) / 60).ToString("00");
        string seconds = (elapsedTime % 60).ToString("00");

        timerText.text = minutes + ":" + seconds;
    }

    public int GetCurrentLevel()
    {
        return currentLevel; // This method can be used to retrieve the number of minutes passed
    }
}
