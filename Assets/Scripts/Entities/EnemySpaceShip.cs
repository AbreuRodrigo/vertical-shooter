using UnityEngine;
using System.Collections;

public class EnemySpaceShip : SpaceShip {
	private GameWorld game;

	public GameObject bullet;
	public Gun gun;

	private int secondsTillShooting = 2;
	private float counter = 0;

	void Awake() {
		game = GameObject.FindObjectOfType<GameWorld>();
		gun.AddShootEvent(this.OnShoot);
	}

	void FixedUpdate() {
		if (!GameWorld.IsPaused()) {
			transform.Translate (new Vector2 (0, -1) * Time.deltaTime * (speed + Random.Range(0.1f, 1.5f)));
									
			if(counter >= secondsTillShooting) {
				counter = 0;
				
				if(Random.Range(0, 100) < GameWorld.percentEnemyShooting) {
					gun.Shoot();
				}
			} else {
				counter += Time.deltaTime;
			}
		}
	}

	protected override void OnShoot() {
		GameObject b = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
		b.GetComponent<Rigidbody2D>().AddForce(new Vector2 (0, -5), ForceMode2D.Impulse);
	}
	
	public override void TakeDamage(int damage) {
		this.lives -= damage;
		
		if(this.lives <= 0){
			this.Explode();
			
			game.SumUpScore();
		}
	}
}