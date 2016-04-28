using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace VerticalShooter {

	//This class represents the GameWorld, pretty much, the most important class in the game, responsible for holding all the 
	//logics and main gameobjects, controling the game status (pause, gameover etc). It also holds the game constants
	public class GameWorld : MonoBehaviour {		
		private static GameState State { get; set; }
		private GameState PreviousState { get; set; }

		//This is meant to be the instance of the playerSpaceShip, the spaceship controlled by the player
		public PlayerSpaceShip player;

		//As the gameLevel increses
		private int gameLevel = 1;

		//How many points does the player win for destroying an enemy
		public static int enemyAwards = 10;
		//The intial chance for an enemy fire its gun
		public static int percentEnemyShooting = 25;

		void Start() {
			//Setting up the life icon in the UI for each of the player's life
			GUIController.instance.SetUpLivesHud (player.lives);

			//Everytime the game scene is loaded, the gamelevel resets to 1, enemyAwards to 10 and percentEnemyShooting to 25
			gameLevel = 1;
			enemyAwards = GameRulesConfig.ENEMY_AWARDS_INCREMENT;
			percentEnemyShooting = GameRulesConfig.INITIAL_ENEMY_SHOOTING_PERCENT;

			//If the game is coming from the GameOver State, then goes direct to GamePlayState by choosing to play again
			if (State.Equals (GameState.GameOver)) {
				//Changing the game state to GamePlay and setting up the UI for showing the gamePlay properly
				ChangeToGamePlayState ();
			} else {//Else, if the game state is different from GameOver, it's coming from the Menu

				//Deactivating the playerSpaceShip object
				player.gameObject.SetActive (false);

				//Changing the game state to GameMenu and setting up the UI for showing the menu properly
				ChangeToGameMenuState ();

				//Coroutine for postponing a method invocation. Passing the amount in seconds and method to be invoked after those seconds
				//In this case the method passed is responsible for showing the MenuUI animation
				StartCoroutine(PostponeActionInvocation(1f, GUIController.instance.ShowMenuUI));
			}
		}

		void Update () {
			//Processing the player inputs for each game frame
			ProcessInputs ();
		}

		private void ProcessInputs () {
			//In this case, the unique input available for the player is the escape button, for quitting the game
			if (Input.GetKeyDown (KeyCode.Escape)) {
				Application.Quit ();
			}
		}

		//This method is the onClick action for the PlayBtn, located in the hierarchy under GamePlayCanvas>GameMenuPlay>PlayBtn
		public void PlayGameButtonPress() {
			//Calling the global and unique object responsible for playing sounds, in this case, calling the LifeSound
			SoundController.instance.PlayLifeUpSound ();
			//Changing the game state to GamePlay and setting up the UI for showing the gamePlay properly
			ChangeToGamePlayState ();
		}

		//This method is the onClick action for the PauseBtn, located in the hierarchy under GamePlayCanvas>PauseBtn
		public void PauseGameButtonPress() {
			//Calling the global and unique object responsible for playing sounds, in this case, calling the LifeSound
			SoundController.instance.PlayLifeUpSound ();
			//Changing the game state to GamePause and setting up the UI for showing the pause screen properly
			ChangeToGamePauseState ();
		}

		//This method is the onClick action for the RetakeBtn, located in the hierarchy under 
		//GamePlayCanvas>GamePauseDialog>PauseContainer>RetakeBtn
		public void RetakeGameButtonPress() {
			//Calling the global and unique object responsible for playing sounds, in this case, calling the LifeSound
			SoundController.instance.PlayLifeUpSound ();
			//Changing the game state to GamePlay and setting up the UI for showing the gamePlay properly
			ChangeToGamePlayState ();
		}

		//This method is the onClick action for the PlayAgainBtn, located in the hierarchy under
		//GamePlayCanvas>GameOverDialog>GameOverContainer>PlayAgainBtn
		public void PlayAgainButtonPress() {
			//Calling the global and unique object responsible for playing sounds, in this case, calling the LifeSound
			SoundController.instance.PlayLifeUpSound ();
			//Calling the ScreenFader to show the fading out effect
			GUIController.instance.ScreenFadeOut ();
			//Postponing the ReloadGameScene method to be executed after 1 second
			StartCoroutine(PostponeActionInvocation(1f, ReloadGameScene));
		}

		//Changes the game state to GameMenu and setup the UI configuration accordingly
		public void ChangeToGameMenuState() {
			//Defining the current game state as GameMenu
			State = GameState.GameMenu;
			//Setting up the UI to show/hide the properly elements for the GameMenu
			GUIController.instance.ConfigUIForGameMenuState ();
		}

		//Changes the game state to GamePause and setup the UI configuration accordingly
		public void ChangeToGamePauseState() {
			//Defining the current game state as GamePause
			State = GameState.GamePause;
			//Setting up the UI to show/hide the properly elements for the GamePause state
			GUIController.instance.ConfigUIForGamePauseState ();
			//Calling the PauseDialog animation to show it in the UI 
			GUIController.instance.ShowPauseDialog ();
			//Postponing the stopTimeScale method, to effectively pause the game after 0.7 seconds
			StartCoroutine(PostponeActionInvocation(0.7f, StopTimeScale));
		}

		//Changes the game state to GamePlay and setup the UI configuration accordingly
		public void ChangeToGamePlayState() {
			//Keeping track of the current game state before changing it to GamePlay state
			PreviousState = State;
			//Defining the current game state as GamePlay
			State = GameState.GamePlay;

			//Testing if the previous game state was gamePause
			if(PreviousState.Equals(GameState.GamePause)) {
				//Resets the timeScale to 1, so the game unpauses
				UnleashTimeScale ();
				//After unpausing the game, it hides the pauseDialog
				GUIController.instance.HidePauseDialog ();
			}

			//Setting up the UI to show/hide the properly elements for the GamePlay state
			GUIController.instance.ConfigUIForGamePlayState ();

			//Testing if the previous game state was gameMenu
			if(PreviousState.Equals(GameState.GameMenu)) {
				//Hiding the menu dialog by calling its hide animation
				GUIController.instance.HideMenuUI ();
			}

			//Guaranteeing the playerSpaceShip object is not null to avoid nullpointer exception
			if(player != null) {
				//Activating the playerSpaceShip object
				player.gameObject.SetActive (true);
			}
		}

		//Changes the game state to GameOver and setup the UI configuration accordingly
		public void ChangeToGameOverState() {
			//Defining the current game state as GameOver
			State = GameState.GameOver;

			//Setting up the UI to show/hide the properly elements for the GameOver state
			GUIController.instance.ConfigUIForGameOverState ();
			//Calling the GameOverDialog animation to show it in the UI 
			GUIController.instance.ShowGameOverDialog ();
		}

		//Sums the current enemy awards to the player's total score
		public void SumUpScore () {
			//Updating the values of score/highscore in the UI accordingly to the player's current total score 
			GUIController.instance.UpdateScoreUIWithEnemyAwards ();
			//Processes the level-up criterias to update the game objects/variables accordingly
			ProcessLevelUp ();
		}

		//Tests if the current game state is GameMenu
		public static bool IsMenu() {
			return State.Equals (GameState.GameMenu);
		}

		//Tests if the current game state is GamePlay
		public static bool IsPlaying() {
			return State.Equals (GameState.GamePlay);
		}

		//Tests if the current game state is GamePause
		public static bool IsPaused () {
			return State.Equals (GameState.GamePause);
		}

		//Tests if the current game state is GameOver
		public static bool IsGameOver () {
			return State.Equals (GameState.GameOver);
		}

		//Gives the player an extra life, updating the UI accordingly
		public void GivePlayerAnExtraLife () {
			//Testing whether the number of lives of the playerSpaceShip is lower than the configured life limit.
			if (player.lives < GameRulesConfig.PLAYER_MAX_LIVES) {
				//Calling the global UIController to add an extra life icon to the UI accordingly the player's total life number
				GUIController.instance.AddExtraLifeToPlayerHud (player.lives);
			}
		}

		//Stops the time scale used for pausing the game
		private void StopTimeScale() {
			Time.timeScale = 0;
		}

		//Resets the time scale to 1, retaking the game
		private void UnleashTimeScale() {
			Time.timeScale = 1;
		}

		//Reload the GAMEPLAY_NAME scene
		private void ReloadGameScene() {
			SceneManager.LoadScene (GameRulesConfig.GAMEPLAY_NAME);
		}

		//Processes the level-up criterias to update the game objects/variables accordingly
		private void ProcessLevelUp () {
			//Retrieves the player's current total score from UIController as an integer
			int points = GUIController.instance.GetCurrentScore ();

			/* Testing the 3 criterias for conceding an extra life to player:
			 * 1- Whenever the total score is a multiple of POINTS_TO_LEVEL_UP (100 by default)
			 * 2- Whenever the player's total life number is less than the max lives permitted
			 * 3- Whenever the total score when divided by POINTS_TO_LEVEL_UP (100 by default), 
			 *    is greater than or equal to the current gamelevel
			 */
			if ((points % GameRulesConfig.POINTS_TO_LEVEL_UP) == 0 && player.lives < GameRulesConfig.PLAYER_MAX_LIVES && 
				(points / GameRulesConfig.POINTS_TO_LEVEL_UP) >= gameLevel) {
				//Calling the global/unique soundController to play the lifeUp soundFX
				SoundController.instance.PlayLifeUpSound ();

				//If the player's total life number is greater than zero, then...
				if(player.lives > 0) {
					//Give the player an extra life
					GivePlayerAnExtraLife ();
					//Increment the gamelevel by 1
					gameLevel++;
					//Increment the player lives by 1
					player.lives++;
				}
				//If the chance for enemies to shoot their guns is less than the configured ENEMY_SHOOT_PERCENT_LIMIT (90 by default)
				if (percentEnemyShooting < GameRulesConfig.ENEMY_SHOOT_PERCENT_LIMIT) {
					//Then, increments the enemies shooting chance by ENEMY_SHOOT_PERCENT_BY_LEVEL (5 by default)
					percentEnemyShooting += GameRulesConfig.ENEMY_SHOOT_PERCENT_BY_LEVEL;
				}
			}

			//If the player's total current score is a multiple of MORE_ENEMY_AWARDS (500 by default) AND
			//the enemy's award is less than MAX_AWARDS (30 by default), then...
			if (points % GameRulesConfig.MORE_ENEMY_AWARDS == 0 &&
				enemyAwards < GameRulesConfig.MAX_AWARDS) {
				//Increments the enemy's awards by ENEMY_AWARDS_INCREMENT (10 by default)
				enemyAwards += GameRulesConfig.ENEMY_AWARDS_INCREMENT;
			}
		}

		//Coroutine for postponing an action/method invocation by x seconds, where x is 'waitSeconds', which is received by args
		IEnumerator PostponeActionInvocation (float waitSeconds, System.Action action) {
			//Waits X seconds
			yield return new WaitForSeconds (waitSeconds);

			//Guaranteeing the action received is not null to avoid nullpointer exception when calling it's method 'Invoke'
			if(action != null) {
				//Invoking the received method as an action
				action.Invoke ();
			}
		}
	}
}