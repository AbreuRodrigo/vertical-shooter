namespace VerticalShooter {
	
	public class GameRulesConfig {
		//Whenever a player makes 100 points more, an extra life is earned
		public const int POINTS_TO_LEVEL_UP = 100;
		//Whenever a player makes 500 points more, the enemy awards increases by 10
		public const int MORE_ENEMY_AWARDS = 500;
		//This is the maximum awards for destroying enemies, when the player scores 1000 points, so the enemies are worth 30 points
		public const int MAX_AWARDS = 30;
		//This is the percent for increasing the chance of an enemy fire its gun
		public const int ENEMY_SHOOT_PERCENT_BY_LEVEL = 5;
		//The maximum chance for an enemy fire its gun
		public const int ENEMY_SHOOT_PERCENT_LIMIT = 90;
		//The maximum amount of lives a player can keep
		public const int PLAYER_MAX_LIVES = 8;
		//The increment of enemy awards as the game level increases
		public const int ENEMY_AWARDS_INCREMENT = 10;
		//This is the initial percent for an enemy fire its gun
		public const int INITIAL_ENEMY_SHOOTING_PERCENT = 25;
		//Constant holding the scene name
		public const string GAMEPLAY_NAME = "GamePlay";
	}
}