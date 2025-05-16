using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] float health = 10f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Projectile")
        {
            health -= 1f;
        }
    }

    void Update()
    {
        if (health == 0)
        {
            Time.timeScale = 0;
        }
    }
}
