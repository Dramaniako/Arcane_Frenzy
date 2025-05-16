using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject player;
    public Vector3 playerPosition;
    public float speed = 0.01f;
    public int health = 3;
    public bool grounded;
    void Update()
    {
        if (health == 0)
        {
            Destroy(gameObject);
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
