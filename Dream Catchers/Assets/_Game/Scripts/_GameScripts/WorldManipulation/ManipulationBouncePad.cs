﻿///=====================================================================================
/// Author: Matt
/// Purpose: Creates bounce pads upon world change
///======================================================================================

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class ManipulationBouncePad : ManipulationScript
{
    public GameObject player; // The player to act upon
    private PlayerMachine machine;

    public Animator bouncePadAnim;

    public float bounceMaxHeight;
    public float bounceMinHeight;
    public float bounceAcceleration;

    public bool bounceInDream;
    public bool bounceInNightmare;

    private bool bounceObject;
    private float originalMaxHeight;
    private float originalMinHeight;
    private float originalAcceleration;

    private float jumpReset = 0.5f;

    // Use this for initialization
    void Start()
    {
        // Set the default world state
        currentManipType = MANIPULATION_TYPE.OTHER;
        player = GameObject.FindGameObjectWithTag("Player");
        machine = player.GetComponent<PlayerMachine>();
        originalMaxHeight = machine.MaxJumpHeight;
        originalMinHeight = machine.MinJumpHeight;
        originalAcceleration = machine.JumpAcceleration;

        setBounce();
    }

    public override void changeState(ManipulationManager.WORLD_STATE state)
    {
        currentObjectState = state;

        setBounce();
    }

    public void setBounce()
    {
        if (currentObjectState == ManipulationManager.WORLD_STATE.DREAM && bounceInDream)
        {
            bounceObject = true;
        }
        else if (currentObjectState == ManipulationManager.WORLD_STATE.NIGHTMARE && bounceInNightmare)
        {
            bounceObject = true;
        }
        else
        {
            bounceObject = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (bounceObject && other.gameObject == player)
        {
            machine.Bounce(bounceMaxHeight, bounceMinHeight, bounceAcceleration);

            gameObject.SendMessage("Play", SendMessageOptions.DontRequireReceiver);

            if (bouncePadAnim != null)
            {
                bouncePadAnim.SetTrigger("Bounce");
            }
        }
    }
}
