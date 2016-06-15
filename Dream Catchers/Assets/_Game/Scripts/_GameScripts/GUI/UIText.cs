﻿using UnityEngine;
using System.Collections;

public class UIText : MonoBehaviour {

    public string sentence;
    public float time;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            InGameStats igs = GameObject.FindObjectOfType<InGameStats>();
            if (igs != null)
            {
                igs.ShowTextInBox(sentence, time); 
            } 
        }
    }
}