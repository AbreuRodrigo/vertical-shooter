using UnityEngine;
using System.Collections;

namespace VerticalShooter {
	
	//This class represents the bullet fired by spaceships
	public abstract class Bullet : VerticalShooterDisposableObject {
		protected string target;//Target name
		protected int damageFactor = 1;//Amount of damage it causes after colliding

		//When a bullet collides into an object that meets its target type, 
		//then it applies its damage factor to the targeted object
		void OnTriggerEnter2D (Collider2D otherCollider) {
			if (otherCollider.tag == target) {
				//Applying the bullet damage factor
				otherCollider.GetComponent<SpaceShip> ().TakeDamage (damageFactor);

				//Deactivate and destroys this bullet
				DeactivateAndDestroy ();
			}
		}
	}
}