using UnityEngine;
using System.Collections;

namespace VerticalShooter {

	//This singleton class is responsible for playing sounds
	public class SoundController : MonoBehaviour {
		//This is the singleton basis, a public static instance of this same class
		public static SoundController instance;

		//This is an AudioClip containing the shoot soundFX
		public AudioClip shoot;

		//This is an AudioClip containing the explosion soundFX
		public AudioClip explosion;

		//This is an AudioClip containing the lifeUp soundFX
		public AudioClip lifeUp;

		void Awake () {
			//If the static instance is not already defined, then it's defined using 'this' reference
			if (instance == null) {
				instance = this;
			}
		}

		//This is the wrapper for playing the shoot AudioClip using the PlaySound method
		public void PlayShootSound () {
			//Testing if shoot is not null to avoid passing a null reference
			if (shoot != null) {
				PlaySound (shoot);
			}
		}

		//This is the wrapper for playing the explosion AudioClip using the PlaySound method
		public void PlayExplosionSound () {
			//Testing if explosion is not null to avoid passing a null reference
			if (explosion != null) {
				PlaySound (explosion);
			}
		}

		//This is the wrapper for playing the lifeUp AudioClip using the PlaySound method
		public void PlayLifeUpSound () {
			//Testing if lifeUp is not null to avoid passing a null reference
			if (lifeUp != null) {
				PlaySound (lifeUp);
			}
		}

		//Plays a given AudioClip passed by args
		private void PlaySound (AudioClip originalClip) {
			//Plays the received audioClip using the current camera as emission point
			AudioSource.PlayClipAtPoint (originalClip, Camera.main.transform.position);
		}
	}
}