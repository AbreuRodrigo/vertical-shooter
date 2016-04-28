using UnityEngine;
using System.Collections;

namespace VerticalShooter {

	//This class is an abstraction for UI windows
	public abstract class UIDialog : MonoBehaviour {
		
		public Animator myAnimator;

		//Abstract method to be overridden by te specialized UIDialog for showing the dialog
		public abstract void Show ();

		//Abstract method to be overridden by te specialized UIDialog for hiding the dialog
		public abstract void Hide ();
	}
}