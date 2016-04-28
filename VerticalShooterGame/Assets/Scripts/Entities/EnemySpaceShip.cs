using UnityEngine;
using System.Collections;

namespace VerticalShooter {

	//This specialized spaceship class represents the enemy spaceship in the game
	public class EnemySpaceShip : SpaceShip {
		//GameWorld isntance for calling SumUpScore later when the enemySpaceShip explodes
		private GameWorld game;

		//The bullet prefab to be instantiated while shooting the gun
		public GameObject bulletPrefab;

		//The gun object bound to this spaceship
		public Gun gun;

		private int secondsTillShooting = 2;
		private float counter = 0;

		void Awake () {
			//Initializing the GameWorld instance by finding the current GameWorld object from the scene
			game = GameObject.FindObjectOfType<GameWorld> ();
			//Adding the overriden OnShot method of this spaceship as an event to be invoked when this spaceship's gun is fired
			gun.AddShootEvent (this.OnShoot);
		}

		void Update () {
			//This logic are only runs if the game is not paused
			if (!GameWorld.IsPaused ()) {
				//This is the continuous movement of the enemy spaceship coming from up to bottom
				transform.Translate (new Vector2 (0, -1) * Time.deltaTime * (speed + Random.Range (0.1f, 1.5f)));

				//This is the cooldown to make the enemy spaceship fire it's gun again
				if (counter >= secondsTillShooting) {
					//Reset the counter to keep shooting
					counter = 0;

					//Tests the chance for this spaceship shoot it's gun (starts with 25% of chance)
					if (Random.Range (0, 100) < GameWorld.percentEnemyShooting) {
						//Calling the spaceship OnShoot method which was added as an event during the Awake
						gun.Shoot ();
					}
				} else {
					//Keeps counting till it reaches the moment of possibly shooting again
					counter += Time.deltaTime;
				}
			}
		}

		//This is the method that will be delegated when this spaceship's gun is fired
		protected override void OnShoot () {
			GameObject b = Instantiate (bulletPrefab, transform.position, Quaternion.identity) as GameObject;
			b.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, -5), ForceMode2D.Impulse);
		}

		//This is the method used to damage this spaceship, by passing and int damage value
		public override void TakeDamage (int damage) {
			//Deducting the received damage from this spaceship's total lives
			this.lives -= damage;
		
			//If this spaceship's life is dicreased to zero or less it's detroyed
			if (this.lives <= 0) {
				//Calls the Explode implementation
				this.Explode ();
				//Increases the score of the player who destroyed this spaceship
				game.SumUpScore ();
			}
		}

		//This is the implementation of the Explode method, common to all enemy spaceships at this time
		public override void Explode() {
			//Calling the global effectController to generate an explosion effect in the same place where this spaceship was destroyed
			EffectsController.instance.CreateEffect (transform.position, EffectType.Explosion, 2);

			//Calling the glocal soundController to play the explosion soundFX
			SoundController.instance.PlayExplosionSound ();

			//Calling the glocal effectController again, bu this time passing the ScoreUp effect, to create the score animation in the 
			//place where this spaceship was destroyed
			EffectsController.instance.CreateEffect (transform.position, EffectType.ScoreUp, 1);

			//Deactivate and destroys this current spcaceship
			DeactivateAndDestroy ();
		}
	}
}