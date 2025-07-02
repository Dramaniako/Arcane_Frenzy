using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public GameManager gameManager;


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Projectile")
        {
            gameManager.playerStats.health -= 1f;
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
