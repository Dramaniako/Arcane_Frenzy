using UnityEngine;
using UnityEngine.UIElements;

public class Door : MonoBehaviour
{
    public GameManager gameManager;
    public bool down = false;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.killCount == 1 && down == false)
        {
            transform.Translate(0f, -50f, 0f);
            down = true;
        }
    }
}
