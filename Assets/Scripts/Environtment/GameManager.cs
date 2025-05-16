using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public float spawnRadius = 5f;
    void Start()
    {
        StartCoroutine(Spawn());
    }
    IEnumerator Spawn()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(3);
            // Generate a random position around the spawner
            Vector2 randomOffset2D = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset2D.x, 0f, randomOffset2D.y);

            // Instantiate the enemy
            GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);

            // Assign the player reference
            newEnemy.GetComponent<EnemyBehaviour>().player = player;
        }
    }
}
