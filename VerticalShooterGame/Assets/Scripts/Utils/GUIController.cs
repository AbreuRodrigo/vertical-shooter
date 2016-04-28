using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace VerticalShooter {

	//This singleton class is responsible for handling UI, UI elements and its main functionalities
	public class GUIController : MonoBehaviour {
		//This is the list of lifeIcons in the UI
		private List<Image> HUDLives = new List<Image> ();

		//This is the singleton basis, a public static instance of this same class
		public static GUIController instance;
		public int HighScore { get; set; }

		//Bellow are all the required UI or related elements, they were annotated as SerializeField to be available via inspector
		[SerializeField]
		private GameOverDialog gameOverDialog;
		[SerializeField]
		private GamePauseDialog gamePauseDialog;
		[SerializeField]
		private GameMenuPanel gameMenuPanel;
		[SerializeField]
		private Button pauseBtn;
		[SerializeField]
		private Joystick joystickInterface;
		[SerializeField]
		private Transform livesContainer;
		[SerializeField]
		private Image lifeImg;
		[SerializeField]
		private Text scoreLabel;
		[SerializeField]
		private Text scoreVal;
		[SerializeField]
		private Text highscoreLabel;
		[SerializeField]
		private Text highscoreVal;
		[SerializeField]
		private ScreenFader screenFader;

		void Awake () {
			//If the static instance is not already defined, then it's defined using 'this' reference
			if (instance == null) {
				instance = this;
			}
			//Turns on/off the joystick depending on the current platform
			ConfigJoystickInterfaceByPlatform ();
		}

		//Returns the current text score from the UIText element as an integer
		public int GetCurrentScore() {
			return int.Parse (scoreVal.text);
		}

		//Called for showing the gameOverDialog
		public void ShowGameOverDialog () {
			//Testing if not null to avoid null pointer exception
			if (gameOverDialog != null) {
				//Calling the gameOverDialog Show method, which plays the 'Show' animation
				gameOverDialog.Show ();
			}
		}

		//Updates the Score and Highscore in the UI
		public void UpdateScoreUIWithEnemyAwards() {
			//If the UI.Text scoreVal is not null, then...
			if (scoreVal != null) {
				//Assign the current valid enemy awards to the current player's score
				int s = GetCurrentScore() + GameWorld.enemyAwards;
				scoreVal.text = s.ToString ();

				//If the current highscoreVal (UI.Text) is not null, and the current new score (s) is greater than the current Highscore
				if (highscoreVal != null && s > HighScore) {
					//Updates the current highscore UI with the same value as the current score
					highscoreVal.text = s.ToString ();
					//If the player has got a new higher score, then the highscore UI becomes green
					highscoreVal.color = Color.green;
				}
			}
		}

		public void SetUpLivesHud (int playerLives) {
			if (livesContainer != null && lifeImg != null) {
				for (int i = 0; i < playerLives; i++) {
					AddExtraLifeToHud (new Vector3 (0, lifeImg.rectTransform.rect.position.y - (i * lifeImg.rectTransform.rect.height), 0));
				}
			}
		}

		public void ShowScoreUI() {
			scoreLabel.enabled = true;
			scoreVal.enabled = true;
		}

		public void HideScoreUI() {
			scoreLabel.enabled = false;
			scoreVal.enabled = false;
		}

		public void ShowHighscoreUI() {
			highscoreLabel.enabled = true;
			highscoreVal.enabled = true;
		}

		public void HideHighscoreUI() {
			highscoreLabel.enabled = false;
			highscoreVal.enabled = false;
		}

		public void ShowLivesContainer() {
			livesContainer.gameObject.SetActive (true);
		}

		public void HideLivesContainer() {
			livesContainer.gameObject.SetActive (false);
		}

		public void ShowPauseDialog() {
			if (gamePauseDialog != null) {
				gamePauseDialog.Show ();
			}
		}

		public void HidePauseDialog() {
			if (gamePauseDialog != null) {
				gamePauseDialog.Hide ();
			}
		}

		public void TogglePauseButtonVisibility(bool active) {
			if(pauseBtn != null) {
				pauseBtn.gameObject.SetActive (active);
			}
		}

		public void ToggleJoystickInterfaceVisibility(bool active) {
			if(joystickInterface != null) {
				joystickInterface.gameObject.SetActive (active);
			}
		}

		public void ScreenFadeOut() {
			if (screenFader != null) {
				screenFader.Enable ();
				screenFader.FadeOut ();
			}
		}

		public void AddExtraLifeToPlayerHud(int playerLives) {
			AddExtraLifeToHud (new Vector3 (0, lifeImg.rectTransform.rect.position.y - (playerLives * lifeImg.rectTransform.rect.height), 0));
		}

		public void TakeOutLifeFromPlayerHud(int playerLives) {
			if (playerLives >= 0) {
				Image img = HUDLives [playerLives];

				HUDLives.RemoveAt (playerLives);

				Destroy (img.gameObject);
			}
		}

		//Analyses the Highscore to apply some logics
		public void AnalyseHighscore () {
			string highscoreKey = "Highscore";

			//Retrieving the preously saved highscore, bringing 0 if the highscoreKey is not already defined
			//OBSERVATION:
			//In a real game it would be a bad idea to store highscore locally, because it easier to modify, instead we can also
			//use some cloud service or considering using an online server to validate the last saved highscore
			int saved = PlayerPrefs.GetInt (highscoreKey, 0);
			//If the retrieved highscore is 0, then it may use 1000 by default
			int firstHighscore = 1000;

			int currentScore = GetCurrentScore ();

			if (saved != 0) {
				HighScore = saved < currentScore ? currentScore : saved;
			} else {
				HighScore = firstHighscore < currentScore ? currentScore : firstHighscore;
			}

			PlayerPrefs.SetInt (highscoreKey, HighScore);

			if (highscoreVal != null) {
				highscoreVal.text = HighScore.ToString ();
			}
		}

		public void HideMenuUI() {
			//Testing if gameMenuPanel is not null to avoid null pointer exception
			if(gameMenuPanel != null) {
				//Calling gameMenuPanel show method to run the 'Hide' animation
				gameMenuPanel.Hide ();
			}
		}

		public void ShowMenuUI() {
			//Testing if gameMenuPanel is not null to avoid null pointer exception
			if(gameMenuPanel != null) {
				//Calling gameMenuPanel show method to run the 'Show' animation
				gameMenuPanel.Show ();
			}
		}

		//Config the UI, turning on/off the UI elements properly for the GameMenu state
		public void ConfigUIForGameMenuState() {
			HideScoreUI ();
			HideHighscoreUI ();
			TogglePauseButtonVisibility (false);
			ToggleJoystickInterfaceVisibility (false);
			HideLivesContainer ();
		}

		//Config the UI, turning on/off the UI elements properly for the GamePlay state
		public void ConfigUIForGamePlayState() {
			AnalyseHighscore ();
			ShowScoreUI ();
			ShowHighscoreUI ();
			TogglePauseButtonVisibility (true);
			ShowLivesContainer ();
			ConfigJoystickInterfaceByPlatform ();
		}

		//Config the UI, turning on/off the UI elements properly for the GamePause state
		public void ConfigUIForGamePauseState() {
			HideScoreUI ();
			HideHighscoreUI ();
			TogglePauseButtonVisibility (false);
			ToggleJoystickInterfaceVisibility (false);
			HideLivesContainer ();
		}

		//Config the UI, turning on/off the UI elements properly for the GameOver state
		public void ConfigUIForGameOverState() {
			ShowGameOverDialog ();
			AnalyseHighscore ();
			TogglePauseButtonVisibility (false);
			ToggleJoystickInterfaceVisibility (false);
			HideLivesContainer ();
		}

		//Turns on/off the joystick depending on the current platform
		private void ConfigJoystickInterfaceByPlatform() {
			#if UNITY_ANDROID
				ToggleJoystickInterfaceVisibility(true);
			#else
				ToggleJoystickInterfaceVisibility(false);
			#endif
		}

		//Receives a position modifier for generating a new LifeIcon in the GUI when the player receives an extra life
		private void AddExtraLifeToHud (Vector3 modPosition) {
			//The original position for the lifeImg 
			Vector3 pos = lifeImg.transform.position;

			//incrementing the origin lifeImg position by the received position modifier
			pos += modPosition;

			//Instantiating a new lifeImg using the lifeImg reference as prefab and the new incremented position (pos)
			//OBSERVATION:
			//This algorithm can be optimized by creating an objectPool during the Awake, to avoid runtime instantiation
			Image life = (Image)Instantiate (lifeImg, pos, lifeImg.transform.rotation);
			//Setting the new instantiated lifeImge's parent as the livesContainer received via inspector 
			life.transform.SetParent (livesContainer, false);

			//Add the current life image to the HUDLives list
			HUDLives.Add (life);
		}
	}
}