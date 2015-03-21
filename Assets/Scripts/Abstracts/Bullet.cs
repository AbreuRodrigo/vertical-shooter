using UnityEngine;
using System.Collections;

public abstract class Bullet : MonoBehaviour {
	public Vector2 speed;
	protected string target;

	void OnTriggerEnter2D(Collider2D otherCollider) {
		if (otherCollider.tag == target) {
			gameObject.SetActive(false);

			otherCollider.GetComponent<SpaceShip>().TakeDamage(1);

			Destroy(gameObject);
		}
	}
}