using UnityEngine;
using System.Collections;

public abstract class SpaceShip : MonoBehaviour {
	public int life;
	public int force;
	public int level;
	public float speed;
	public float deceleration;

	private Renderer renderer;

	protected abstract void OnShoot();

	public void TakeDamage(int damage) {
		this.life -= damage;

		if(life <= 0){
			this.Explode();
		}
	}

	private void Explode() {
		EffectsController.instance.CreateEffect(transform.position, EffectType.Explosion, 2);
		gameObject.SetActive(false);
		Destroy(gameObject);
	}

	public void LevelUp() {
		this.level++;
	}

	void Awake() {
		renderer = GetComponent<Renderer>();
	}

	void OnBecameInvisible() {
		Destroy(this.gameObject);
	}
}