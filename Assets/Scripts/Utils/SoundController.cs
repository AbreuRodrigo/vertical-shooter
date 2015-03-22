using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {
	public static SoundController instance;

	public AudioClip shoot;
	public AudioClip explosion;
	public AudioClip lifeUp;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
	}

	public void PlayShootSound() {
		if (shoot != null) {
			PlaySound(shoot);
		}
	}

	public void PlayExplosionSound() {
		if (explosion != null) {
			PlaySound(explosion);
		}
	}

	public void PlayLifeUpSound() {
		if (lifeUp != null) {
			PlaySound(lifeUp);
		}
	}

	private void PlaySound(AudioClip originalClip){
		AudioSource.PlayClipAtPoint(originalClip, Camera.main.transform.position);
	}
}