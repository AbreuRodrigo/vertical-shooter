using UnityEngine;
using System.Collections;

namespace VerticalShooter
{
	//This class represents the spaceships in the game
	public abstract class SpaceShip : VerticalShooterDisposableObject {
		public int lives = 1;//How many lives does this spaceship have
		public int level = 1;//What's the current level of this spaceship
		public float speed = 10.0f;
		public float deceleration = 0.05f;

		//Abstract method to be overridden by te specialized spaceship for shooting bullets
		protected abstract void OnShoot ();

		//Abstract method to be overridden by the specialized spaceship for taking damage
		public abstract void TakeDamage (int damage);

		//Abstract method to be overridden by the specialized spaceship for exploding this spaceship
		public abstract void Explode ();

		void OnTriggerEnter2D (Collider2D otherCollider) {
			//Try getting a spaceship type from the collided object
			SpaceShip other = otherCollider.GetComponent<SpaceShip> ();

			//If the other object retrieved during the trigger collision is also a spaceship, 
			//then the current spaceship receives 1 point of damage
			if (other != null) {
				TakeDamage (1);
			}
		}

		//Method that should be called for making this spaceship increase its level, so it becoming more powerful
		public void LevelUp () {
			level++;
		}
	}
}