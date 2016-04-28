using UnityEngine;
using System.Collections;

namespace VerticalShooter {

	//This class represents the gun of any spaceship
	public class Gun : MonoBehaviour {
		//The cooldown counts continuously the time for shooting this gun again, after the shooting rate has passed
		private float shootCooldown;
		//This is the shooting rate used to define the interval for shooting this gun
		public float shootingRate = 0.3f;

		//This delegate is used for mixing shooting actions defined at the spaceship
		public delegate void ShootAction ();

		public event ShootAction actions;

		void Start () {
			//Starts the shootCoolDown in 0 seconds
			shootCooldown = 0f;
		}

		void Update () {
			//While shootCoolDown is bigger than 0, then decrements the cooldown each frame
			if (shootCooldown > 0) {
				shootCooldown -= Time.deltaTime;
			}
		}

		//This method is called when this gun is fired
		public void Shoot () {
			//This gun can only attack when the shootCoolDown is less than or equal to zero
			if (CanAttack) {
				//Resetting the shootCooldown for the shootingRate again
				shootCooldown = shootingRate;
				//This is the invocation of the OnShoot method added to this events during the spaceship's Awake
				actions ();
				//Calling the global and unique SoundController available and plays the gunShoot sound
				SoundController.instance.PlayShootSound ();
			}
		}

		//Method used for adding actions to the gun shoot event
		public void AddShootEvent (ShootAction action) {
			this.actions = action;
		}

		//Tests whether or not this gun can attack. When the ShootCooldown is less than or equal to zero, then it returns true
		public bool CanAttack {
			get {
				return (shootCooldown <= 0f);
			}
		}
	}
}