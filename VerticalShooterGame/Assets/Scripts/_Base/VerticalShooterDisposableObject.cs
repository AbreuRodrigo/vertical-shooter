using UnityEngine;
using System.Collections;

namespace VerticalShooter {	

	public class VerticalShooterDisposableObject : MonoBehaviour {
		//Called when a renderer goes out of the screen
		void OnBecameInvisible () {
			DeactivateAndDestroy ();
		}

		//Deactivate and destroy this object reference
		protected void DeactivateAndDestroy () {
			gameObject.SetActive (false);
			Destroy (gameObject);
		}
	}
}