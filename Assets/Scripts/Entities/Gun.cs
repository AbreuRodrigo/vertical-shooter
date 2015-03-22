using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
	private float shootCooldown;
	public float shootingRate = 0.3f;

	public delegate void ShootAction();
	public event ShootAction actions;
	
	void Awake() {
		shootCooldown = 0f;
	}
	
	void Update() {
		if (shootCooldown > 0) {
			shootCooldown -= Time.deltaTime;
		}
	}
	
	public void Shoot() {
		if (CanAttack) {
			shootCooldown = shootingRate;
			actions();
			SoundController.instance.PlayShootSound();
		}
	}
				
	public void AddShootEvent(ShootAction action){
		this.actions = action;
	}
	
	public bool CanAttack {
		get {
			return (shootCooldown <= 0f);
		}
	}
}
