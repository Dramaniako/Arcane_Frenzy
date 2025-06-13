using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject Magic_Bullet;
    [SerializeField] GameObject Ice_Bullet;
    [SerializeField] GameObject Lightning;
    [SerializeField] Transform firePoint;  // Where bullet spawns (e.g., in front of camera)
    [SerializeField] Camera playerCamera;
    public Attack attack;
    private GameObject Prefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireBullet();
        }

        int spell = GetComponent<Attack>().currentSpell.spellID;

        switch (spell)
        {
            case 1:
                Prefab = Magic_Bullet;
                break;
            case 2:
                Prefab = Ice_Bullet;
                break;
            case 3:
                Prefab = Lightning;
                break;

            default:
                Debug.Log("Spell ID not found");
                break;
        }
    }

    public void FireBullet()
    {
        // 1. Use firePoint's forward direction as the shooting direction
        Vector3 direction = firePoint.forward;

        // 2. Instantiate bullet facing that direction
        GameObject bullet = Instantiate(Prefab, firePoint.position, Quaternion.LookRotation(direction));

        // 3. Launch the bullet in that direction
        bullet.GetComponent<Projectile>().Launch(direction);

    }
}
