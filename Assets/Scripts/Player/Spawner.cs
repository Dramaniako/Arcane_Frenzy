using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;  // Where bullet spawns (e.g., in front of camera)
    [SerializeField] Camera playerCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireBullet();
        }
    }

    void FireBullet()
    {
        // 1. Use firePoint's forward direction as the shooting direction
        Vector3 direction = firePoint.forward;

        // 2. Instantiate bullet facing that direction
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));

        // 3. Launch the bullet in that direction
        bullet.GetComponent<MagicBullet>().Launch(direction);

    }
}
