using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI level;
    public TextMeshProUGUI money;
    public TextMeshProUGUI health;


    public PlayerStats playerStats;
    public GameManager gameManager;

    void Update()
    {
        level.text = "Level" + gameManager.level.ToString();
        money.text = playerStats.money.ToString() + " $";
        health.text = "Health: " + playerStats.health.ToString();
    }
}
