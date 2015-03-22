using UnityEngine;
using System.Collections;

public abstract class Bullet : MonoBehaviour {
	private Renderer renderer;

	protected string target;

	public Vector2 speed;

	void Awake() {
		renderer = GetComponent<Renderer>();
	}

	void OnTriggerEnter2D(Collider2D otherCollider) {
		if (otherCollider.tag == target) {
			gameObject.SetActive(false);

			otherCollider.GetComponent<SpaceShip>().TakeDamage(1);

			Destroy(gameObject);
		}
	}

	void OnBecameInvisible() {
		Destroy(gameObject);
	}
}