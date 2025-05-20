using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    [SerializeField] float disappear = 10f;
    public float magnitude;
    public Rigidbody rigidBody;
    public bool exploding = false, exploded = false;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        StartCoroutine(TimedExplosion());
    }

    void Update()
    {
        if (exploding && !exploded)
        {
            StartCoroutine(Wait());
            rigidBody.constraints = RigidbodyConstraints.FreezePosition;
        }
        if (exploding)
        {
            exploded = true;
            Explode();
        }
    }

    public void Launch(Vector3 direction)
    {
        GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);
    }

    IEnumerator TimedExplosion()
    {
        yield return new WaitForSeconds(disappear);
        exploding = true;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(magnitude);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            exploding = true;
        }
    }

    void Explode()
    {
        transform.localScale += new Vector3(magnitude, magnitude, magnitude);
    }

}
