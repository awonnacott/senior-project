using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {
    public PlayerHealth playerHealth;
    public GameObject[] enemies;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;

    void Start () {
		StartCoroutine (Spawn ());
    }

	IEnumerator Spawn () {
		yield return new WaitForSeconds (spawnTime);
		while (true) {
			if (playerHealth.currentHealth <= 0f)
				yield break;
			int enemyIndex = Random.Range (0, enemies.Length);
			int spawnPointIndex = Random.Range (0, spawnPoints.Length);
			Instantiate (enemies [enemyIndex], spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation);
			yield return new WaitForSeconds (spawnTime *= 0.98f);
		}
    }
}
