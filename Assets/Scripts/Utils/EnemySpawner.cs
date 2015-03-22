using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public EnemySpaceShip enemy;
	public int enemiesPerSecond = 1;
	public bool generateEnemies = true;

	private float max;
	private float min;
	private Vector2 enemySize;

	void Awake() {
		enemySize = enemy.GetComponent<BoxCollider2D>().size * 0.5f;

		min = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, 0)).x + enemySize.x;
		max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - enemySize.x;

		Random.seed = System.Environment.TickCount;
	}

	void Start() {
		StartCoroutine("GenerateEnemies");
	}

	IEnumerator GenerateEnemies() {
		while(generateEnemies && enemy != null){
			yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));

			float x = Random.Range(min + enemySize.x, max - enemySize.x);

			Vector3 position = new Vector3(x, 10, 0);

			Instantiate(enemy, position, Quaternion.identity);
		}
	}
}
