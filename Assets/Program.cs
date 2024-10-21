using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using System.Collections;

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
		health = Clamp(health + Clamp(hp, maxHealth), maxHealth);
	}

	// heal player shield up to maxShield, by parameter hp
	public void RegenerateShield(int hp) {
		shield = Clamp(shield + Clamp(hp, maxShield), maxShield);
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

	// ---------------------------------------------------------------------------- debug tests

	// ------------------------------------------------------ damage

	public static void Test_TakeDamage_Shield() {
		var test = new HealthSystem();
		test.ResetGame();
		test.TakeDamage(10);
		Debug.Assert(90 == test.shield, $"shield is actually {test.shield}");
		Debug.Assert(100 == test.health, $"health is actually {test.health}");
	}
	public static void Test_TakeDamage_HealthShield() {
		var test = new HealthSystem();
		test.ResetGame();
		test.TakeDamage(150);
		Debug.Assert(0 == test.shield, $"shield is actually {test.shield}");
		Debug.Assert(50 == test.health, $"health is actually {test.health}");
	}	
	public static void Test_TakeDamage_ShieldDepleted() {
		var test = new HealthSystem();
		test.ResetGame();
		test.shield = 0;
		test.TakeDamage(25);
		Debug.Assert(0 == test.shield, $"shield is actually {test.shield}");
		Debug.Assert(75 == test.health, $"health is actually {test.health}");
	}	
	public static void Test_TakeDamage_HealthZero() {
		var test = new HealthSystem();
		test.ResetGame();
		test.shield = 0;
		test.TakeDamage(100);
		Debug.Assert(100 == test.shield, $"shield is actually {test.shield}");
		Debug.Assert(100 == test.health, $"health is actually {test.health}");
		Debug.Assert(2 == test.lives, $"lives is actually {test.lives}");
	}
	public static void Test_TakeDamage_ShieldHealthZero() {
		var test = new HealthSystem();
		test.ResetGame();
		test.TakeDamage(200);
		Debug.Assert(100 == test.shield, $"shield is actually {test.shield}");
		Debug.Assert(100 == test.health, $"health is actually {test.health}");
		Debug.Assert(2 == test.lives, $"lives is actually {test.lives}");
	}
	public static void Test_TakeDamage_Negative() {
		var test = new HealthSystem();
		test.ResetGame();
		test.TakeDamage(-10);
		Debug.Assert(100 == test.health, "Negative number damage wasn't ignored");
	}

	// ------------------------------------------------------ heal

	public static void Test_Heal_Normal() {
		var test = new HealthSystem();
		test.ResetGame();
		test.health = 50;
		test.Heal(25);
		Debug.Assert(75 == test.health, $"health is actually {test.health}");
	}	
	public static void Test_Heal_Full() {
		var test = new HealthSystem();
		test.ResetGame();
		test.Heal(25);
		Debug.Assert(100 == test.health, $"health is actually {test.health}");
	}
	public static void Test_Heal_Negative() {
		var test = new HealthSystem();
		test.ResetGame();
		test.Heal(-25);
		Debug.Assert(100 == test.health, $"health is actually {test.health}");
	}	
	
	// ------------------------------------------------------ shield

	public static void Test_Shield_Normal() {
		var test = new HealthSystem();
		test.ResetGame();
		test.shield = 50;
		test.RegenerateShield(25);
		Debug.Assert(75 == test.shield, $"shield is actually {test.shield}");
	}	
	public static void Test_Shield_Full() {
		var test = new HealthSystem();
		test.ResetGame();
		test.RegenerateShield(25);
		Debug.Assert(100 == test.shield, $"shield is actually {test.shield}");
	}
	public static void Test_Shield_Negative() {
		var test = new HealthSystem();
		test.ResetGame();
		test.RegenerateShield(-25);
		Debug.Assert(100 == test.shield, $"shield is actually {test.shield}");
	}

	// ------------------------------------------------------ revive

	public static void Test_Revive() {
		var test = new HealthSystem();
		test.ResetGame();
		test.shield = 0;
		test.health = 0;
		test.Revive();
		Debug.Assert(100 == test.shield, $"shield is actually {test.shield}");
		Debug.Assert(100 == test.health, $"health is actually {test.health}");
		Debug.Assert(2 == test.lives, $"lives is actually {test.lives}");
	}

	// ------------------------------------------------------ xp

	public static void Test_XP_Normal() {
		var test = new HealthSystem();
		test.ResetGame();
		test.xp = 20;
		test.IncreaseXP(40);
		Debug.Assert(60 == test.xp, $"xp is actually {test.xp}");
	}
	public static void Test_XP_LvlUp() {
		var test = new HealthSystem();
		test.ResetGame();
		test.xp = 80;
		test.IncreaseXP(20);
		Debug.Assert(0 == test.xp, $"xp is actually {test.xp}");
		Debug.Assert(2 == test.level, $"xp is actually {test.level}");
	}	
	public static void Test_XP_Negative() {
		var test = new HealthSystem();
		test.ResetGame();
		test.xp = 50;
		test.IncreaseXP(-20);
		Debug.Assert(50 == test.xp, $"xp is actually {test.xp}");

	}

	// call every test above
	public static void RunAllUnitTests() {

		Test_TakeDamage_Shield();
		Test_TakeDamage_HealthShield();
		Test_TakeDamage_ShieldDepleted();
		Test_TakeDamage_HealthZero();
		Test_TakeDamage_ShieldHealthZero();
		Test_TakeDamage_Negative();

		Test_Heal_Normal();
		Test_Heal_Full();
		Test_Heal_Negative();

		Test_Shield_Normal();
		Test_Shield_Full();
		Test_Shield_Negative();

		Test_Revive();

		Test_XP_Normal();
		Test_XP_LvlUp();
		Test_XP_Negative();
	}
}