using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : PersistentSingleton<PlayerManager>
{
    public int speed;
    [Range(0, 40)] public int maxSpeed;
    public int health;
    [Range(0, 100)] public int maxHealth;
    public int stamina;
    [Range(0, 100)] public int maxStamina;

    void Start()
    {
        health = maxHealth;   
        stamina = maxStamina;
        speed = maxSpeed/2;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
