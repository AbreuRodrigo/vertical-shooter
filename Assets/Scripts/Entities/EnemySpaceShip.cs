using UnityEngine;
using System.Collections;

public class EnemySpaceShip : SpaceShip {

	protected override void OnShoot() {
	}

	void FixedUpdate() {
		transform.Translate(new Vector2(0, -1) * Time.deltaTime * speed);
	}
}
