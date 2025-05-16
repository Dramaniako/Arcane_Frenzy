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
        GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(disappear);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
