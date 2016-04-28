using UnityEngine;
using System.Collections;

namespace VerticalShooter {
	
	public class GameMenuPanel : UIDialog {

		public override void Show() {
			if (myAnimator != null) {
				myAnimator.Play ("Show");
			}
		}

		public override void Hide() {
			if (myAnimator != null) {
				myAnimator.Play ("Hide");
			}
		}
	}
}