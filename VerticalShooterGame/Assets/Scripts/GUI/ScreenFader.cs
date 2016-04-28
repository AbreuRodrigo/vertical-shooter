using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace VerticalShooter {

	//This is an UI class used for controlling the game screen fadeOut (only for now) effect
	public class ScreenFader : MonoBehaviour {
		//The animator component
		public Animator myAnimator;
		//The image component, which will be animated for the fadeOut effect
		public Image myImage;

		//Wraps the animator's play animation method for fadingOut effects
		public void FadeOut() {
			if(myAnimator != null) {
				myAnimator.Play ("FadeOut");
			}
		}

		//Disables the animator and image components and then deactivates this gameObject
		public void Disable() {
			myAnimator.enabled = false;
			myImage.enabled = false;
			gameObject.SetActive (false);
		}

		//Enables the animator and image components and then activates this gameObject
		public void Enable() {
			myAnimator.enabled = true;
			myImage.enabled = true;
			gameObject.SetActive (true);
		}
	}
}