﻿using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {

    public int maxHealth;
    public int currentHealth;

    bool isVulnerable = true;
	
	// Update is called once per frame
	void Update () {
	
        if(currentHealth < 0)
        {
            //TODO: Kill Animation
            Destroy(gameObject);
        }
	}

    public void TakeDamage(int dmg)
    {
        if(isVulnerable)
        {
            currentHealth -= dmg;
        }
    }

    public void Heal(int heal)
    {
        currentHealth += heal;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}