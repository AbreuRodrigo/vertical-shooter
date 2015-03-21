using UnityEngine;
using System.Collections;

public class PlayerBullet : Bullet {

	void Awake() {
		target = "Enemy";
	}
}
