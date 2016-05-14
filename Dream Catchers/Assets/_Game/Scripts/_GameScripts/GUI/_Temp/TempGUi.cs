﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TempGUi : MonoBehaviour {

    public Text Health;
    public Text MemoryFrags;
    public Text OtherCollectibles;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //simple ui test for generic info
        Health.text = " Health: " + Character_Manager.Instance.currentHealth.ToString();
        MemoryFrags.text = " MemoryFragments: " + Character_Manager.Instance.totalMemoryFragmentsCollected.ToString();
        OtherCollectibles.text = " Other: " + Character_Manager.Instance.totalCollectibles.ToString();

        if(Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("MainMenu");
        }

	}
}
