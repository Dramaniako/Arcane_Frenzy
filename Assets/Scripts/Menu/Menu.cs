using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void Begin()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Start_Menu");
    }
}
