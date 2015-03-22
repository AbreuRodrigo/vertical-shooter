using UnityEngine;
using System.Collections;

public class PlayerSpaceShip : SpaceShip {
	private GameWorld game;
	private Vector2 colliderSize;
	private float minX, maxX, minY, maxY;

	public GameObject bullet;
	public Gun gun;

	Vector3 inputTransform;
	Vector3 mouseMov;

	void Awake() {
		speed = 10.0f;
		deceleration = 0.05f;
		gun.AddShootEvent(this.OnShoot);

		colliderSize = GetComponent<BoxCollider2D>().size * 0.5f;

		minX = Camera.main.ScreenToWorldPoint(Vector3.zero).x + colliderSize.x;
		maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - colliderSize.x;
		minY = Camera.main.ScreenToWorldPoint(Vector3.zero).y + colliderSize.y;
		maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - colliderSize.y;

		game = GameObject.FindObjectOfType<GameWorld>();
	}

	void FixedUpdate() {
		if(!GameWorld.IsPaused()) {
			DoInputLogics();

			EnsurePosition();
		}
	}

	void Update() {
		if (!GameWorld.IsPaused()) {
			if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) {
				gun.Shoot ();
			}
		}
	}

	protected override void OnShoot() {
		GameObject b = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
		b.GetComponent<Rigidbody2D>().AddForce(new Vector2 (0, 10), ForceMode2D.Impulse);
	}

	public override void TakeDamage(int damage){
		this.lives -= damage;

		game.TakeOutLifeFromPlayer();
		
		if(this.lives <= 0){
			this.Explode();

			game.DoGameOver();
		}
	}

	void EnsurePosition() {
		Vector3 newPos = transform.position;
		
		newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
		newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
		
		transform.position = newPos;
	}

	void DoInputLogics() {
		if (Input.GetAxis ("Mouse X") != 0 || Input.GetAxis ("Mouse Y") != 0) {
			mouseMov = 
				Camera.main.ScreenToWorldPoint(
					new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10)
				) * Time.deltaTime * speed * 1.2f;
		} else {
			if(mouseMov.x != 0){
				if(mouseMov.x > 0){
					mouseMov.x -= deceleration;
					if(mouseMov.x < 0){
						mouseMov.x = 0;
					}
				}else{
					mouseMov.x += deceleration;
					if(mouseMov.x > 0){
						mouseMov.x = 0;
					}
				}
			}
			if(mouseMov.y != 0){
				if(mouseMov.y > 0){
					mouseMov.y -= deceleration;
					if(mouseMov.y < 0){
						mouseMov.y = 0;
					}
				}else{
					mouseMov.y += deceleration;
					if(mouseMov.y > 0){
						mouseMov.y = 0;
					}
				}
			}
		}
		
		inputTransform = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) + mouseMov;
								
		transform.Translate(inputTransform * Time.deltaTime * speed);
	}
}