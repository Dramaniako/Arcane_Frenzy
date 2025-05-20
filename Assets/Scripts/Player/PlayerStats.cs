using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health = 10f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Projectile")
        {
            health -= 1f;
        }
    }
}
