﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectibleSpawner : MonoBehaviour {

    public GameObject Prefabs;
    public float probabilityOfDrop;
    HealthManager HM;
     GameObject spawns;
     //public string name;

	// Use this for initialization
	void Start () {
	
        if(probabilityOfDrop > 1)
        {
            probabilityOfDrop = probabilityOfDrop / 100;
        }

        spawns = Instantiate(Prefabs);
        if(name != "" || name == null)
        {
            spawns.name = this.name;
        }

        HM = GetComponent<HealthManager>();
	}

    //Item drop when the object is destroyed
    void OnDestroy()
    {
        float rand;
        rand = Random.value;
        if(rand < probabilityOfDrop && HM != null)
        {
            if(HM.spawns == true)
            {
                Instantiate(spawns, this.transform.position, this.transform.rotation);
            }
            
        }
           
        
    }
}
