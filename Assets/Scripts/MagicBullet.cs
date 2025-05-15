using System.Collections;
using UnityEngine;

public class MagicBullet : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    [SerializeField] float disappear = 1;
    private Rigidbody rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        StartCoroutine(Wait());
    }

    public void Launch(Vector3 direction)
    {
        rigidBody.linearVelocity = direction.normalized * speed;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(disappear);
        Destroy(gameObject);
    }
}
