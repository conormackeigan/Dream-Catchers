﻿using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ManipulationGravity : ManipulationScript {

    enum FLOAT_STATE
    {
        UP = 0,
        DOWN = 1,
        NEUTRAL = 2
    }

    FLOAT_STATE currentFloat = FLOAT_STATE.DOWN;

    public float maxDist;
    public float minDist;
    public float timeToFloat;

    public bool floatInDream;
    public bool floatInNightmare;

    // Use this for initialization
    void Start()
    {
        // Set the default world state
        currentManipType = MANIPULATION_TYPE.OTHER;

    }

    void FixedUpdate()
    {
        if(currentFloat == FLOAT_STATE.DOWN)
        {
            FloatDown();
        }
        else
        {
            FloatUp();
        }
    }

    // TODO: Combine into a single function
    void FloatUp()
    {
        Vector3 dir = new Vector3(0, -1, 0);
        RaycastHit hit;

        int layerMask = 1 << 9; // Only float Above the level mask

        Debug.DrawRay(transform.position, dir * maxDist);

        Physics.Raycast(transform.position, dir, out hit, layerMask); // The Raycast to calculate distance from floor

        if (hit.distance < maxDist && hit.distance > 0 && !DOTween.IsTweening(transform)) 
        {
            // Move by offset distance
            Vector3 toMove = new Vector3(transform.position.x, transform.position.y + (maxDist - hit.distance), transform.position.z);
            transform.DOMove(toMove, timeToFloat);
        }
    }


    void FloatDown()
    {
        Vector3 dir = new Vector3(0, -1, 0);
        RaycastHit hit;

        int layerMask = 1 << 9; // Only float Above the level mask

        Debug.DrawRay(transform.position, dir * maxDist);

        Physics.Raycast(transform.position, dir, out hit, layerMask); // The Raycast to calculate distance from floor

        if (hit.distance != minDist && !DOTween.IsTweening(transform))
        {
            // Move by offset distance
            Vector3 toMove = new Vector3(transform.position.x, transform.position.y + (minDist - hit.distance), transform.position.z);
            transform.DOMove(toMove, timeToFloat);
        }
    }

    public override void changeState(ManipulationManager.WORLD_STATE state)
    {
        currentObjectState = state;

        if (currentObjectState == ManipulationManager.WORLD_STATE.DREAM && floatInDream)
        {
            currentFloat = FLOAT_STATE.UP;
        }
        else if (currentObjectState == ManipulationManager.WORLD_STATE.NIGHTMARE && floatInNightmare)
        {
            currentFloat = FLOAT_STATE.UP;
        }
        else
        {
            currentFloat = FLOAT_STATE.DOWN;
        }

    }

}
