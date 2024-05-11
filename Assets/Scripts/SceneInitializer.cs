using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneInitializer : MonoBehaviour
{
    public Slider pointsBar;
    public TMP_Text levelText;
    public GameObject levelUpMenu;
    public GameObject player;
    void Start()
    {
        PointsSystem.Instance.SetUIReferences(pointsBar, levelText, levelUpMenu, player);
    }
}
