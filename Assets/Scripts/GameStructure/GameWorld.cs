using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameWorld : MonoBehaviour {
	private List<Image> HUDLives = new List<Image>();

	public static GameState State { get; set; }
	public static int HighScore { get; set;}

	public PlayerSpaceShip player;
	public Transform uiCanvas;
	public Image lifeImg;
	public Text scoreVal;
	public Text highscoreVal;

	private int gameLevel = 1;

	public static int enemyAwards = 10;
	public static int percentEnemyShooting = 25;

	private const int POINTS_TO_LEVEL_UP = 100;
	private const int POINTS_TO_MORE_AWARDS = 500;
	private const int MAX_AWARDS = 30;
	private const int ENEMY_SHOOT_PERCENT_BY_LEVEL = 5;
	private const int ENEMY_SHOOT_PERCENT_LIMIT = 90;
	private const int PLAYER_MAX_LIVES = 10;

	void Awake() {
		State = GameState.GamePlay;
		SetUpLivesHud();
		AnalyseHighscore();
	}

	void Update() {
		ProcessInputs();
	}

	private void ProcessInputs() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			TogglePause();
		}
	}

	public void TogglePause(){
		if (State.Equals(GameState.Pause)) {
			Unpause();
		} else if(State.Equals(GameState.GamePlay)) {
			Pause();
		}
	}

	private void Pause() {
		StartCoroutine(WaitSecondsAndPause(0.5f));
	}

	private void Unpause() {
		StartCoroutine(WaitSecondsAndUnpause(0));
	}

	private void SetUpLivesHud() {
		if(uiCanvas != null && lifeImg != null){
			for(int i = 0; i < player.lives; i++){
				AddExtraLifeToHud(new Vector3(0, i * lifeImg.transform.position.y * 1.5f, 0));
			}
		}
	}

	public void DoGameOver() {
		State = GameState.GameOver;

		GUIController.instance.ShowGameOverDialog();

		AnalyseHighscore();
	}

	public void ReloadGame() {
		Application.LoadLevel("GamePlay");
	}

	public void SumUpScore() {
		if (scoreVal != null) {
			int s = int.Parse(scoreVal.text) + enemyAwards;
			scoreVal.text = s.ToString ();

			if(highscoreVal != null && s > HighScore) {
				highscoreVal.text = s.ToString();
				highscoreVal.color = Color.green;
			}
		}

		ProcessLevelUp();
	}

	public void AddExtraLifeToHud(Vector3 modPosition) {
		if (player.lives < PLAYER_MAX_LIVES) {
			Vector3 pos = lifeImg.transform.position;
			
			pos += modPosition;
			
			Image life = (Image)Instantiate (lifeImg, pos, lifeImg.transform.rotation);
			life.transform.SetParent (uiCanvas, false);
			
			HUDLives.Add(life);
		}
	}

	public void GivePlayerAnExtraLife() {
		if(player.lives < PLAYER_MAX_LIVES){
			AddExtraLifeToHud(new Vector3(0, player.lives * lifeImg.transform.position.y * 1.5f, 0));

			player.lives++;
		}
	}

	public void TakeOutLifeFromPlayer() {
		if(player.lives >= 0){
			Image img = HUDLives[player.lives];

			HUDLives.RemoveAt(player.lives);

			Destroy(img.gameObject);
		}
	}

	public static bool IsPaused() {
		return State.Equals(GameState.Pause);
	}

	public static bool IsGameOver() {
		return State.Equals(GameState.GameOver);
	}

	private void ProcessLevelUp() {
		int points = int.Parse(scoreVal.text);

		if((points % POINTS_TO_LEVEL_UP) == 0 && 
		    player.lives < PLAYER_MAX_LIVES && 
		    (points / POINTS_TO_LEVEL_UP) >= gameLevel) {
			SoundController.instance.PlayLifeUpSound();
			
			GivePlayerAnExtraLife();
			
			gameLevel++;
			
			if(percentEnemyShooting < ENEMY_SHOOT_PERCENT_LIMIT){
				percentEnemyShooting += ENEMY_SHOOT_PERCENT_BY_LEVEL;
			}
		}

		if(points % POINTS_TO_MORE_AWARDS == 0 && 
		    enemyAwards < MAX_AWARDS) {
			enemyAwards += 10;
		}
	}

	private void AnalyseHighscore() {
		string highscoreKey = "Highscore";

		int saved = PlayerPrefs.GetInt(highscoreKey, 0);
		int firstHighscore = 1000;

		int currentScore = int.Parse(scoreVal.text);

		if(saved != 0) {
			HighScore = saved < currentScore ? currentScore : saved;
		} else {
			HighScore = firstHighscore < currentScore ? currentScore : firstHighscore;
		}

		PlayerPrefs.SetInt(highscoreKey, HighScore);

		if(highscoreVal != null){
			highscoreVal.text = HighScore.ToString();
		}
	}

	IEnumerator WaitSecondsAndPause(float seconds) {
		State = GameState.Pause;
		
		GUIController.instance.ShowPauseDialog();

		yield return new WaitForSeconds(seconds);

		Time.timeScale = 0;
	}

	IEnumerator WaitSecondsAndUnpause(float seconds) {
		yield return new WaitForSeconds(seconds);

		State = GameState.GamePlay;
		
		Time.timeScale = 1;
		
		GUIController.instance.HidePauseDialog();
	}
}