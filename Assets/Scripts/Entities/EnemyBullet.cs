using UnityEngine;
using System.Collections;

public class EnemyBullet : Bullet {

	void Awake() {
		target = "Player";
	}
}
