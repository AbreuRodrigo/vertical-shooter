using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {
	public static GUIController instance;

	public Animator gameOverDialog;
	public Animator pauseDialog;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
	}

	public void ShowGameOverDialog() {
		if(gameOverDialog != null) {
			gameOverDialog.Play("ScaleIn");
		}
	}

	public void ShowPauseDialog() {
		if(pauseDialog != null) {
			pauseDialog.Play("ScaleIn");
		}
	}

	public void HidePauseDialog() {
		if(pauseDialog != null) {
			pauseDialog.Play("ScaleOut");
		}
	}
}