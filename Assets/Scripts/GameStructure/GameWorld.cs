using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameWorld : MonoBehaviour {
	private List<Image> lives = new List<Image>();
	private GameState State { get; set; }

	public PlayerSpaceShip player;
	public Transform uiCanvas;
	public Image lifeImg;
	public Text scoreVal;
	public int maxLives = 10;

	void Awake() {
		State = GameState.GamePlay;
		SetUpLivesHud();
	}

	void Update() {
		ProcessInputs();
	}

	private void ProcessInputs() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			TogglePause();
		}
	}

	private void TogglePause(){
		if (State.Equals(GameState.Pause)) {
			Unpause();
		} else {
			Pause();
		}
	}

	private void Pause() {
		State = GameState.Pause;
		Time.timeScale = 0;
	}

	private void Unpause() {
		State = GameState.GamePlay;
		Time.timeScale = 1;
	}

	private void SetUpLivesHud() {
		if(uiCanvas != null && lifeImg != null){
			for(int i = 0; i < player.lives; i++){
				AddExtraLifeToHud(new Vector3(0, i * lifeImg.transform.position.y * 1.5f, 0));
			}
		}
	}

	private void AddExtraLifeToHud(Vector3 modPosition){
		if (player.lives < maxLives) {
			Vector3 pos = lifeImg.transform.position;
			
			pos += modPosition;
			
			Image life = (Image)Instantiate (lifeImg, pos, lifeImg.transform.rotation);
			life.transform.SetParent (uiCanvas, false);
			
			lives.Add (life);
		}
	}

	public void DoGameOver() {
		Unpause();		
		State = GameState.GameOver;
		GUIController.instance.ShowGameOverDialog();
	}

	public void ReloadGame() {
		Application.LoadLevel("GamePlay");
	}

	public void SumUpScore(int score) {
		int s = int.Parse(scoreVal.text) + score;
		scoreVal.text = s.ToString();
	}

	public void GivePlayerAnExtraLife() {
		if(player.lives < maxLives){
			AddExtraLifeToHud(new Vector3(0, player.lives * lifeImg.transform.position.y * 1.5f, 0));

			player.lives++;
		}
	}

	public void TakeOutLifeFromPlayer() {
		if(player.lives >= 0){
			Image img = lives[player.lives];

			lives.RemoveAt(player.lives);

			Destroy(img.gameObject);
		}
	}
}