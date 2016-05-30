﻿using UnityEngine;
using System.Collections;

public class HammerAttack : MonoBehaviour {

    public PlayerCombat combatScript;
    public int hammerDamage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && combatScript.attacking)
        {
            other.BroadcastMessage("TakeDamage", hammerDamage);
        }
    }
}
