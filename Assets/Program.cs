using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public class HealthSystem
{
	// Variables
	public int health;
	public int shield;	
	public int lives;
	public string healthStatus;

	const int maxHealth = 100;
	const int maxShield = 100;
	const int startLives = 3;
	const int startLevel = 1;

	// Optional XP system variables
	public int xp;
	public int level;

	public HealthSystem() {
		ResetGame();
	}

	// helper function for range clamping
	private int Clamp(int num, int max) {
		return Math.Max(0, Math.Min(num, max));
	}

	// returns hud display as a string
	public string ShowHUD() {
		var healthStatus = new Dictionary<int, string>{
			{ 90, "Perfect Health" },
			{ 75, "Healthy" },
			{ 50, "Hurt" },
			{ 10, "Badly Hurt" },
			{ 1, "Imminent Danger" },
			{ 0, "Dead" }
		};
		// Find the key (threshold) that is less than or equal to the health value
		var statusKey = healthStatus.Keys.FirstOrDefault(key => health >= key);

		return $"Health: {health} | Shield: {shield} | Lives: {lives} | Status: {healthStatus[statusKey]} | Level: {level} | XP: {xp}";
	}

	// damage player by amount given, damaging shield first then hp 
	public void TakeDamage(int damage) {
		int shieldDamage = Clamp(damage, shield);
		shield -= shieldDamage;
		health -= Clamp(damage - shieldDamage, health);

		// try to revive player when health reaches 0
		if (health <= 0) Revive();
	}

	// heal player health up to maxHealth, by parameter hp
	public void Heal(int hp) {
		health = Clamp(health + hp, maxHealth);
	}

	// heal player shield up to maxShield, by parameter hp
	public void RegenerateShield(int hp) {
		shield = Clamp(shield + hp, maxShield);
	}

	// revive player if they have lives to spare, otehrwise set game to dead state
	public void Revive() {
		lives--;
		if (lives > 0) {
			health = maxHealth;
			shield = maxShield;
		}
		else {
			health = 0;
			shield = 0;
			lives = 0;
		}
	}

	// reset all variables to default values
	public void ResetGame() {
		health = maxHealth;
		shield = maxShield;
		lives = startLives;
		level = startLevel;
		xp = 0;
	}

	// increases xp and level accordingly
	public void IncreaseXP(int exp) {
		xp += Math.Max(0, exp);
		level += xp / 100;
		xp %= 100;
	}

	// ------------------------------------------------ debug tests

	//public void Test_TakeDamage_OnlyShield() {
	//	HealthSystem system = new HealthSystem();
	//	system.shield = 100;
	//	system.health = 100;
	//	system.lives = 3;

	//	system.TakeDamage(10);

	//	Debug.Assert(90 == system.shield);
	//	Debug.Assert(100 == system.health);
	//	Debug.Assert(3 == system.lives);
	//}
}