using UnityEngine;
using System.Collections;

namespace VerticalShooter {

	//This is a specialized type of SpaceShip, which is controlled by the player
	public class PlayerSpaceShip : SpaceShip {

		//GameWorld isntance for calling SumUpScore later when the enemySpaceShip explodes
		private GameWorld game;

		//This is the spaceship's colliderSize, represented by a Vector2
		private Vector2 colliderSize;

		//This will be used to keep the player inside the screen, representing each side of the current game screen
		private float left, right, top, bottom;

		//The bullet prefab to be instantiated while shooting the gun
		public GameObject bulletPrefab;
		//The gun object bound to this spaceship
		public Gun gun;

		private Vector3 inputTransform;
		private Vector3 playerMov;

		//The joystick interface for playing the game in mobile
		private Joystick joystick;

		void Awake () {
			//Defining the player speed
			speed = 10.0f;
			//Defining the player deceleration
			deceleration = 0.05f;
			//Adding the overriden OnShot method of this spaceship as an event to be invoked when this spaceship's gun is fired
			gun.AddShootEvent (this.OnShoot);
			//Defining the colliderSize to half the BoxCollider2D real size
			colliderSize = GetComponent<BoxCollider2D> ().size * 0.5f;

			//Defining each side of the screen considering the colliderSize of the spaceship  for sums or subtractions, 
			//because, actually, the 0,0,0 point of an object in a 3D Cartesian plane is located in the center, by definition
			left = Camera.main.ViewportToWorldPoint (Vector3.zero).x + colliderSize.x;
			right = Camera.main.ViewportToWorldPoint (Vector3.one).x - colliderSize.x;
			top = Camera.main.ViewportToWorldPoint (Vector3.zero).y + colliderSize.y;
			bottom = Camera.main.ViewportToWorldPoint (Vector3.one).y - colliderSize.y;

			//Searching the inspector for finding the first object of the type GameWorld and Joystick
			game = GameObject.FindObjectOfType<GameWorld> ();
			joystick = GameObject.FindObjectOfType<Joystick> ();
		}

		void FixedUpdate () {
			//This game logics will keep running each frame, while the game is not paused
			if (!GameWorld.IsPaused ()) {
				
				//Defining inputLogics for Android or Desktop, accordingly to the current game platform
				#if UNITY_ANDROID
					DoInputLogicsMobile ();
				#else
					DoInputLogicsDesktop ();	
				#endif

				//fixes the playerScapeShip position to make sure it's inside the screen
				EnsurePosition ();
			}
		}

		void Update () {
			//This game logics will keep running each frame, while the game is not paused
			if (!GameWorld.IsPaused ()) {
				
				//Defining inputLogics for Android or Desktop, accordingly to the current game platform
				#if UNITY_ANDROID
					//If the current platform is Android, then the playerSpaceShip will shoot when any part of the screen is touched
					//If the current platform is Android, but it's actually in the editor, so it uses the mouse button press for shooting
					if(Input.touches.Length != 0 || Input.GetMouseButton (0)) {
						//Calling the shoot method of this spaceship's gun
						gun.Shoot ();
					}
				#else
					//If it's not Android platform, so the playerSpaceShip shoots it's gun by pressing the space button 
					//or the left button in the mouse
					if (Input.GetKey (KeyCode.Space) || Input.GetMouseButton (0)) {
						//Calling the shoot method of this spaceship's gun
						gun.Shoot ();
					}
				#endif
			}
		}

		//This is the method that will be delegated when this spaceship's gun is fired
		protected override void OnShoot () {
			GameObject b = Instantiate (bulletPrefab, transform.position, Quaternion.identity) as GameObject;
			b.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 10), ForceMode2D.Impulse);
		}

		//This is the implementation of the method for dealing damage to the player's spaceship
		public override void TakeDamage (int damage) {
			//Deducting the damage received via args from the playerSpaceship's total lives
			this.lives -= damage;

			//This is calling the global and unique GUIController available for removing from the UI list, the icon that represents
			//the lost life
			GUIController.instance.TakeOutLifeFromPlayerHud (lives);
		
			//If the player spaceship lives is less than or equal to 0, it will be destroyed
			if (this.lives <= 0) {
				//Calling this spaceship's implementation of explosion
				this.Explode ();
				//When the player is destroyed, then change the game state to GameOver
				game.ChangeToGameOverState ();
			}
		}

		//This is the implementation of the playerSpaceShip Explode method
		public override void Explode() {
			//Make use of the EffectsController for creating the explosion effect in a given position with 2 seconds of duration
			EffectsController.instance.CreateEffect (transform.position, EffectType.Explosion, 2);

			//Make use of the SoundController for playing the explosion soundFX
			SoundController.instance.PlayExplosionSound ();

			//Deactivate and destroys this current spcaceship
			DeactivateAndDestroy ();
		}

		//Clamps the position considering each side of the screen, to make sure the playerSpaceShip never goes out side the screen
		void EnsurePosition () {
			Vector3 newPos = transform.position;
		
			newPos.x = Mathf.Clamp (newPos.x, left, right);
			newPos.y = Mathf.Clamp (newPos.y, top, bottom);
		
			transform.position = newPos;
		}

		//Input logics for moving the playerSpaceShip using the joystick interface (Android for now, but may also work for IOS)
		void DoInputLogicsMobile() {
			inputTransform = new Vector3 (joystick.Coordinates.x * 0.7f, joystick.Coordinates.y * 0.7f, 0);

			transform.Translate (inputTransform * Time.deltaTime * speed);			
		}

		//Input logics for moving the playerSpaceShip using the Mouse and Horizontal and Verticla exis (Desktop, Web platforms)
		void DoInputLogicsDesktop () {
			if (Input.GetAxis ("Mouse X") != 0 || Input.GetAxis ("Mouse Y") != 0) {
				playerMov = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10))
					* Time.deltaTime * speed * 1.2f;
			} else {
				if (playerMov.x != 0) {
					if (playerMov.x > 0) {
						playerMov.x -= deceleration;
						if (playerMov.x < 0) {
							playerMov.x = 0;
						}
					} else {
						playerMov.x += deceleration;
						if (playerMov.x > 0) {
							playerMov.x = 0;
						}
					}
				}
				if (playerMov.y != 0) {
					if (playerMov.y > 0) {
						playerMov.y -= deceleration;
						if (playerMov.y < 0) {
							playerMov.y = 0;
						}
					} else {
						playerMov.y += deceleration;
						if (playerMov.y > 0) {
							playerMov.y = 0;
						}
					}
				}
			}
		
			inputTransform = new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0) + playerMov;
											
			transform.Translate (inputTransform * Time.deltaTime * speed);
		}
	}
}