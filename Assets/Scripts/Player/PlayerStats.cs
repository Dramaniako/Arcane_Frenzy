using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public GameManager gameManager;
    public float health = 10f;
    public int money = 0;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Projectile")
        {
            health -= 1f;
        }
        if (collision.gameObject.tag == "Trophy")
        {
            SceneManager.LoadScene("Game_Over");
        }
        if (collision.gameObject.tag == "Start")
        {
            gameManager.rest = false;
            StartCoroutine(gameManager.Spawn());
        }
    }
}
