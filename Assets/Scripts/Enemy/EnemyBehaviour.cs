using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject player;
    public Vector3 playerPosition;
    public GameManager gameManager;
    public float speed = 0.01f;
    public int health = 3;
    public bool grounded;

    void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
    }

    void Update()
    {
        if (health == 0)
        {
            gameManager.killCount += 1;
            Destroy(gameObject);
            gameManager.playerStats.money += 1;
        }

        if (player != null && grounded == true)
        {
            Vector3 targetPos = player.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }

    void LateUpdate()
    {
        playerPosition = player.transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            health -= 1;
        }
        grounded = true;
    }
}
