﻿using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ManipulationRotate : ManipulationScript
{

    public Transform objectTransform;

    public Vector3 rotateDream;
    public Vector3 rotateNightmare;

    public float Duration;

    // Use this for initialization
    void Start()
    {
        // Set the default world state
        currentManipType = MANIPULATION_TYPE.ROTATE;
    }

    public override void changeState(ManipulationManager.WORLD_STATE state)
    {
        currentObjectState = state;

        if (currentObjectState == ManipulationManager.WORLD_STATE.DREAM)
        {
            objectTransform.DORotate(rotateDream, Duration);
        }
        else
        {
            objectTransform.DORotate(rotateNightmare, Duration);
        }
    }
}