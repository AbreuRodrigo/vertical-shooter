using UnityEngine;
using System.Collections;

public class EnemySpaceShip : SpaceShip {

	private GameWorld game;

	public int scoreAwards = 10;

	protected override void OnShoot() {}

	public override void TakeDamage(int damage){
		this.lives -= damage;
		
		if(this.lives <= 0){
			this.Explode();

			game.SumUpScore(scoreAwards);
		}
	}

	void Awake() {
		game = GameObject.FindObjectOfType<GameWorld>();
	}

	void FixedUpdate() {
		transform.Translate(new Vector2(0, -1) * Time.deltaTime * speed);
	}
}
