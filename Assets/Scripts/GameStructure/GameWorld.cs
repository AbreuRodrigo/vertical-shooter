using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameWorld : MonoBehaviour {

	private GameState State { get; set; }

	public PlayerSpaceShip player;

	void Awake() {
		Cursor.visible = false;
		State = GameState.Menu;
	}

	void Update() {

	}

	private void ChangeToMenuState() {
		State = GameState.Menu;
		Unpause();
	}

	private void ChangeToGamePlayState() {
		State = GameState.GamePlay;
		Unpause();
	}

	private void ChangeToPauseState() {
		State = GameState.Pause;
		Pause();
	}

	private void ChangeToGameOverState() {
		State = GameState.GameOver;
		Unpause();
	}

	private void Pause() {
		Time.timeScale = 0;
	}

	private void Unpause() {
		Time.timeScale = 1;
	}
}