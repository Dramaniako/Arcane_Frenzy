using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public GameObject platform;
    public PlayerStats playerStats;
    public int killCount = 0;
    public float spawnRadius = 5f;
    public int level = 1;
    public bool rest = true;

    void Start()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();
        StartCoroutine(Spawn());
    }

    void Update()
    {
        if (rest == false)
        {
            platform.SetActive(false);
        }
        else
        {
            platform.SetActive(true);
        }
        if (playerStats.health <= 0f)
        {
            SceneManager.LoadScene("Game_Over");
        }
        if (killCount == level * 10)
        {
            killCount = 0;
            level += 1;
            rest = true;
        }
    }

    public IEnumerator Spawn()
    {
        if (playerStats.health != 0f && rest == false)
        {
            for (int i = 0; i < level * 10; i++)
            {
                yield return new WaitForSeconds(1 + (3 - level));
                // Generate a random position around the spawner
                Vector2 randomOffset2D = UnityEngine.Random.insideUnitCircle * spawnRadius;
                Vector3 spawnPosition = transform.position + new Vector3(randomOffset2D.x, 0f, randomOffset2D.y);

                // Instantiate the enemy
                GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);

                // Assign the player reference
                newEnemy.GetComponent<EnemyBehaviour>().player = player;
            }
        }
    }
}
