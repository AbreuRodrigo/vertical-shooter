using UnityEngine;
using System.Collections;

public abstract class SpaceShip : MonoBehaviour {
	public int lives = 1;
	public int level = 1;
	public float speed = 10.0f;
	public float deceleration = 0.05f;

	private Renderer renderer;

	protected abstract void OnShoot();

	public abstract void TakeDamage(int damage);

	protected void Explode() {
		EffectsController.instance.CreateEffect(transform.position, EffectType.Explosion, 2);
		this.gameObject.SetActive(false);
		Destroy(this.gameObject);
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

	void OnTriggerEnter2D(Collider2D otherCollider) {
		SpaceShip other = otherCollider.GetComponent<SpaceShip>();

		if (other != null) {
			this.TakeDamage(1);
		}
	}
}