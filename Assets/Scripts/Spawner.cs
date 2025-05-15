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
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Vector3 direction = ray.direction;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<MagicBullet>().Launch(direction);
    }
}
