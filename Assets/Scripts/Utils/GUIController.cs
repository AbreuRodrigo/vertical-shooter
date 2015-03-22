using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {
	public static GUIController instance;

	public Animator gameOverDialog;

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
}