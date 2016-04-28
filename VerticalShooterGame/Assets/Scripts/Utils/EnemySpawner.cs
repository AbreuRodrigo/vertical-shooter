using UnityEngine;
using System.Collections;

namespace VerticalShooter {

	//This class works as the enemy generator
	public class EnemySpawner : MonoBehaviour {
		//This is the object used as reference to generate enemies
		public EnemySpaceShip enemyPrefab;
		//This is the enemy generation rate in seconds, defined as one enemy per second for now
		public int enemiesPerSecond = 1;
		//This is only a test controller to tell the spawner whether or not it must generate enemies
		public bool generateEnemies = true;

		//Min and Max x position where an enemy can be created
		private float max;
		private float min;

		//A vector containing the size of the enemy object in width (x) and height (y)
		private Vector2 enemySize;

		void Awake () {
			//Defining the enemySize with the half collider size
			enemySize = enemyPrefab.GetComponent<BoxCollider2D> ().size * 0.5f;
			//Defining the enemy's level as 1
			enemyPrefab.level = 1;

			//Defining the limits for generating enemies using the camera offset in relation to the world point
			min = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, 0)).x + enemySize.x;
			max = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, 0, 0)).x - enemySize.x;

			//Defining the seed/base for generating random numbers
			Random.seed = System.Environment.TickCount;

			//Runs the coroutine that keeps generating enemies using a time range
			StartCoroutine ("GenerateEnemies");
		}

		//Runs the coroutine that keeps generating enemies using a time range
		IEnumerator GenerateEnemies () {
			while (generateEnemies && enemyPrefab != null) {
				//Waits from 0.5 to 1.5 seconds to generate another enemy
				yield return new WaitForSeconds (Random.Range (0.5f, 1.5f));

				//If the game state equals GamePlay, then...
				if (GameWorld.IsPlaying()) {
					//Randomize the x position that will be used to generate the next enemy, considering min and max x position
					float x = Random.Range (min + enemySize.x, max - enemySize.x);

					//Defining the new enemy's position, using the randomized x and y = 10, for generating an enemy beyond the screen top
					Vector3 position = new Vector3 (x, 10, 0);

					//Instantiating a new enemy using the previous position above
					//OBSERVATION:
					//This algorithm can be optimized by creating an objectPool during the Awake, to avoid runtime instantiation
					Instantiate (enemyPrefab, position, Quaternion.identity);
				}
			}
		}
	}
}