using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is used to store the player's attributes - might be better read in from a scriptable object
// A singleton might be better for this, but this is a quick and dirty way to get it working
public static class PlayerAttributes
{
    public static int plHealth = 100;
    public static int plMaxHealth = 100;
    public static int plSpeed = 100;
    public static int plMaxSpeed = 100;
    public static int plMinSpeed = 20;
    public static int plStamina = 100;
    public static int plMaxStamina = 100;
    public static int plHitPoints = 10;
    public static int plScore = 0;
    public static int plLives = 3;
    public static int plLevel = 1;
    public static int plXP = 0;
    
    public static void Reset()
    {
        plHealth = 100;
        plMaxHealth = 100;
        plSpeed = 100;
        plMaxSpeed = 100;
        plMinSpeed = 20;
        plStamina = 100;
        plMaxStamina = 100;
        plHitPoints = 10;
        plScore = 0;
        plLives = 3;
        plLevel = 1;
        plXP = 0;
    }

    public static void AddHealth(int amount)
    {
        plHealth += amount;
        if (plHealth > plMaxHealth)
        {
            plHealth = plMaxHealth;
        }
    }

    public static void AddSpeed(int amount)
    {
        plSpeed += amount;
        if (plSpeed > plMaxSpeed)
        {
            plSpeed = plMaxSpeed;
        }
    }

    public static void AddStamina(int amount)
    {
        plStamina += amount;
        if (plStamina > plMaxStamina)
        {
            plStamina = plMaxStamina;
        }
    }
    public static void AddDamage(int amount)
    {
        plHitPoints += amount;
    }
    public static void AddScore(int amount)
    {
        plScore += amount;
    }
    public static void AddLives(int amount)
    {
        plLives += amount;
    }

    public static void AddLevel(int amount)
    {
        plLevel += amount;
    }

    public static void AddXP(int amount)
    {
        plXP += amount;
    }

    public static void AddMaxHealth(int amount)
    {
        plMaxHealth += amount;
    }

    public static void AddMaxSpeed(int amount)
    {
        plMaxSpeed += amount;
    }

    public static void AddMaxStamina(int amount)
    {
        plMaxStamina += amount;
    }

    public static void ResetHealth()
    {
        plHealth = plMaxHealth;
    }
    public static void ResetSpeed()
    {
        plSpeed = plMaxSpeed;
    }
    public static void ResetStamina()
    {
        plStamina = plMaxStamina;
    }
    public static void ResetDamage()
    {
        plHitPoints = 10;
    }
}
