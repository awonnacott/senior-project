using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public PlayerHealth playerHealth;
    public GameObject[] enemies;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;

    void Start () {
        InvokeRepeating ("Spawn", spawnTime, spawnTime);
    }

    void Spawn () {
        if (playerHealth.currentHealth <= 0f) return;

		int enemyIndex = Random.Range (0, enemies.Length);
        int spawnPointIndex = Random.Range (0, spawnPoints.Length);

        Instantiate (enemies[enemyIndex], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
