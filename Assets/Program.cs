using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthSystem
{
	// --------------- variables //
	public int health;
	public int shield;	
	public int lives;
	public string healthStatus;
	public int xp;
	public int level;

	// ------------------------ constants //
	const int maxHealth = 100;
	const int maxShield = 100;
	const int startLives = 3;
	const int startLevel = 1;
	const int xpPerLevel = 100;
	const int maxLevel = 99;

	public HealthSystem() {
		ResetGame();
	}

	// ------------------------------------- helper functions //

	// helper function for range clamping
	private int Clamp(int num, int max) {
		return Math.Max(0, Math.Min(num, max));
	}

	// -------------------------------------- gameplay functions //

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

		// find the key that is less than or equal to the health value
		var statusKey = healthStatus.Keys.FirstOrDefault(key => health >= key);

		return $"Health: {health} | Shield: {shield} | Lives: {lives} | Status: {healthStatus[statusKey]} | Level: {level} | XP: {xp}";
	}

	// damage player by amount given, damaging shield first then hp 
	public void TakeDamage(int damage) {
		int shieldDamage = Clamp(damage, shield);
		shield -= shieldDamage;
		health -= Clamp(damage - shieldDamage, health);

		if (health <= 0) Revive();
	}

	// heal player health up to maxHealth, by parameter hp
	public void Heal(int hp) {
		health = Clamp(health + Clamp(hp, maxHealth), maxHealth);
	}

	// heal player shield up to maxShield, by parameter hp
	public void Regen(int hp) {
		shield = Clamp(shield + Clamp(hp, maxShield), maxShield);
	}

	// revive player if they have lives to spare, otherwise set game to dead state
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

	// increases xp and level accordingly
	public void IncreaseXP(int exp) {
		xp += Math.Max(0, exp);
		level = Clamp(level + (xp / xpPerLevel), 99);
		xp %= xpPerLevel;
	}

	// reset all variables to default values
	public void ResetGame() {
		health = maxHealth;
		shield = maxShield;
		lives = startLives;
		level = startLevel;
		xp = 0;
	}

	// -------------------------------------------------------- debug tests //

	// debug helper function
	public static void DebugInt(int q, int a) {
		Debug.Assert(q == a, $"value is {q}, should be {a}");
	}

	 //------------------------------------------ damage //
	public static void Test_TakeDamage_Shield() {
		var test = new HealthSystem();
		test.ResetGame();
		test.TakeDamage(10);
		DebugInt(test.shield, 90);
		DebugInt(test.health, 100);
	}
	public static void Test_TakeDamage_HealthShield() {
		var test = new HealthSystem();
		test.ResetGame();
		test.TakeDamage(150);
		DebugInt(test.shield, 0);
		DebugInt(test.health, 50);
	}	
	public static void Test_TakeDamage_ShieldDepleted() {
		var test = new HealthSystem();
		test.ResetGame();
		test.shield = 0;
		test.TakeDamage(25);
		DebugInt(test.shield, 0);
		DebugInt(test.health, 75);
	}	
	public static void Test_TakeDamage_HealthZero() {
		var test = new HealthSystem();
		test.ResetGame();
		test.shield = 0;
		test.TakeDamage(100);
		DebugInt(test.shield, 100);
		DebugInt(test.health, 100);
		DebugInt(test.lives, 2);
	}
	public static void Test_TakeDamage_ShieldHealthZero() {
		var test = new HealthSystem();
		test.ResetGame();
		test.TakeDamage(200);
		DebugInt(test.shield, 100);
		DebugInt(test.health, 100);
		DebugInt(test.lives, 2);
	}
	public static void Test_TakeDamage_Negative() {
		var test = new HealthSystem();
		test.ResetGame();
		test.TakeDamage(-10);
		DebugInt(test.health, 100);
	}

	// ------------------------------------ heal //

	public static void Test_Heal_Normal() {
		var test = new HealthSystem();
		test.ResetGame();
		test.health = 50;
		test.Heal(25);
		DebugInt(test.health, 75);
	}	
	public static void Test_Heal_Full() {
		var test = new HealthSystem();
		test.ResetGame();
		test.Heal(25);
		DebugInt(test.health, 100);
	}
	public static void Test_Heal_Negative() {
		var test = new HealthSystem();
		test.ResetGame();
		test.Heal(-25);
		DebugInt(test.health, 100);
	}	
	
	// -------------------------------------- shield //

	public static void Test_Shield_Normal() {
		var test = new HealthSystem();
		test.ResetGame();
		test.shield = 50;
		test.Regen(25);
		DebugInt(test.shield, 75);
	}	
	public static void Test_Shield_Full() {
		var test = new HealthSystem();
		test.ResetGame();
		test.Regen(25);
		DebugInt(test.shield, 100);
	}
	public static void Test_Shield_Negative() {
		var test = new HealthSystem();
		test.ResetGame();
		test.Regen(-25);
		DebugInt(test.shield, 100);
	}

	// ------------------------------- revive //

	public static void Test_Revive() {
		var test = new HealthSystem();
		test.ResetGame();
		test.shield = 0;
		test.health = 0;
		test.Revive();
		DebugInt(test.shield, 100);
		DebugInt(test.health, 100);
		DebugInt(test.lives, 2);
	}

	// ---------------------------------- xp //

	public static void Test_XP_Normal() {
		var test = new HealthSystem();
		test.ResetGame();
		test.xp = 20;
		test.IncreaseXP(40);
		DebugInt(test.xp, 60);
	}
	public static void Test_XP_LvlUp() {
		var test = new HealthSystem();
		test.ResetGame();
		test.xp = 80;
		test.IncreaseXP(20);
		DebugInt(test.xp, 0);
		DebugInt(test.level, 2);
	}	
	public static void Test_XP_Negative() {
		var test = new HealthSystem();
		test.ResetGame();
		test.xp = 50;
		test.IncreaseXP(-20);
		DebugInt(test.xp, 50);
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