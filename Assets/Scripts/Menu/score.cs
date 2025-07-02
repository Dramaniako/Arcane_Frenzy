using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class score : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshProUGUI text;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        text.text = "Score: " + playerStats.money.ToString();
    }
}