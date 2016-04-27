using UnityEngine;
using System.Collections;

namespace VerticalShooter {

	/* This specialized bullet class represents bullets fired by player spaceship in the game
	 * 
	 * OBSERVATION: 
	 * For now, the unique difference between specialized bullet classes is it's target,
	 * but I like the idea of modularizing it to make the code cleaner and maintenance centralized
	 */
	public class PlayerBullet : Bullet {
		void Awake () {
			//Define the bullet's target name as 'Enemy'
			target = "Enemy";
		}
	}
}