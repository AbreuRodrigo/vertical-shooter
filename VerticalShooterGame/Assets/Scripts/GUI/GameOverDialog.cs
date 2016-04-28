using UnityEngine;
using System.Collections;

namespace VerticalShooter {
	
	public class GameOverDialog : UIDialog {

		public override void Show() {
			if (myAnimator != null) {
				myAnimator.Play ("ScaleIn");
			}
		}

		public override void Hide() {
			if (myAnimator != null) {
				myAnimator.Play ("ScaleOut");
			}
		}
	}
}