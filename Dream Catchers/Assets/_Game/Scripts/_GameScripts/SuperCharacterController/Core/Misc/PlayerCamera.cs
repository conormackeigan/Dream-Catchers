﻿using UnityEngine;
using System.Collections;


// default camera
public class PlayerCamera : MonoBehaviour
{
    public float MaxDistance = 11.0f;
    public float MinDistance = 9.0f;
    public float Height = 4.5f;

    public float moveSpeed = 10.0f;
    public float rotateSpeed = 100;

    public GameObject PlayerTarget;

    public float idleTimer; // counts how long in seconds the camera transform has been idling
    private Vector3 prevPos;

    private PlayerInputController input;
    private Transform target;
    private PlayerMachine machine;

    // Target:
    public Vector3 targetPosHigh; // above character (standard)
    public Vector3 targetPosLow; // below character (for falling)
    public Vector3 targetPos; // the current desired height

    private SuperCharacterController controller;

    private float yOffset; // offset from default pos by player input

    public float lastGround { get; private set; } // y value of last ground player was on
    public float setLastGround { set { lastGround = value; } }

    // Collision checks:
    public GameObject occlusionCheckL;
    public GameObject occlusionCheckR;

    // EDITOR DEBUG:
    public float crossLength;

	// Use this for initialization
	void Start ()
    {
        input = PlayerTarget.transform.parent.GetComponent<PlayerInputController>();
        machine = PlayerTarget.transform.parent.GetComponent<PlayerMachine>();
        controller = PlayerTarget.transform.parent.GetComponent<SuperCharacterController>();
        target = PlayerTarget.transform;

        lastGround = machine.transform.position.y;

        targetPosHigh = new Vector3(0, 3, 0);
        targetPosLow = new Vector3(0, -0.5f, 0);

        targetPos = targetPosHigh;
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        prevPos = transform.position;

        Debug.DrawLine(transform.position, PlayerTarget.transform.parent.position - (PlayerTarget.transform.parent.position - transform.position).normalized);

        UpdateTarget();

        FollowPlayer();

        // temp rotation test
        if (input.Current.Joy2Input.x != 0)
        {
            transform.RotateAround(target.position, controller.up, Time.deltaTime * rotateSpeed * input.Current.Joy2Input.x);
        }

        if (transform.position == prevPos)
        {
            idleTimer += Time.deltaTime;
        }
        else
        {
            idleTimer = 0;
        }

        //Vector3 targetPos = target.position;
        //targetPos.y = lastGround;

        //transform.position = targetPos; // target.position;

        //Vector3 left = Vector3.Cross(machine.lookDirection, controller.up);

        ////horizontal rotation
        ////transform.rotation = Quaternion.LookRotation(machine.lookDirection, controller.up);

        ////todo: y rotation transform.rotation = Quaternion.AngleAxis(yRotation, left) * transform.rotation;

        //transform.position -= transform.forward * Distance;
        //transform.position += controller.up * Height;
    }


    // moves the player child that the camera follows
    public void UpdateTarget()
    {
        // move target up
        if (PlayerTarget.transform.localPosition.y < targetPos.y)
        {

            Vector3 lerp = Vector3.Lerp(PlayerTarget.transform.localPosition, targetPosHigh, 0.1f);

            if (lerp.y < targetPosHigh.y)
            {
                PlayerTarget.transform.localPosition = lerp;
            }
            else
            {
                PlayerTarget.transform.localPosition = targetPosHigh;
            }
        }
        // move target down
        else if (PlayerTarget.transform.localPosition.y > targetPos.y)
        {
            Vector3 lerp = Vector3.Lerp(PlayerTarget.transform.localPosition, targetPosLow, 0.1f);

            if (lerp.y > targetPosLow.y)
            {
                PlayerTarget.transform.localPosition = lerp;
            }
            else
            {
                PlayerTarget.transform.localPosition = targetPosLow;
            }
        }
    }


    public void FollowPlayer()
    {
        // min/max distance
        Vector3 displacement = Math3d.ProjectVectorOnPlane(controller.up, PlayerTarget.transform.position - transform.position);

        if (displacement.magnitude > MaxDistance)
        {

            transform.position += (displacement.magnitude - MaxDistance) * displacement.normalized; 
        }
        else if (displacement.magnitude < MinDistance)
        {
            transform.position -= (MinDistance - displacement.magnitude) * displacement.normalized;
        }


        // rotation
        //Vector3 angle = (Math3d.ProjectVectorOnPlane(controller.up, PlayerTarget.transform.position - transform.position)).normalized;

        Vector3 angle = (PlayerTarget.transform.position - transform.position).normalized;

        Vector3 cross = Vector3.Cross(transform.forward, angle);

        crossLength = cross.magnitude; // DEBUG

        if (cross.magnitude > 0.1f)
        {
            transform.forward += Vector3.Slerp(transform.forward, PlayerTarget.transform.position - transform.position, 0.05f) * Time.deltaTime * rotateSpeed;
        }

        // y rotation

        

        // up/down movement

    }

    // if something is obstructing the character from camera view, rotate until visible
    public void CheckOcclusion()
    {
        if (idleTimer < 0.6f)
        {
            return; // must be stationary for at least one second to apply automated occlusion rotation
        }

        if (Physics.Raycast(transform.position, (PlayerTarget.transform.parent.position - transform.position).normalized, (PlayerTarget.transform.parent.position - transform.position).magnitude - 1))
        {
            // find out which side to rotate
            Transform left = occlusionCheckL.transform;
            Transform right = occlusionCheckL.transform;

            left.position = transform.position;
            right.position = transform.position;

            bool r = true; // default dir is right

            float distance = 0; // counts how much distance much be travelled to avoid obstruction

            while (true)
            {
                distance += 0.2f;

                left.position = transform.position;
                right.position = transform.position;

                left.RotateAround(PlayerTarget.transform.position, controller.up, -distance);
                right.RotateAround(PlayerTarget.transform.position, controller.up, distance);

                if (!Physics.Raycast(left.position, (PlayerTarget.transform.parent.position - transform.position).normalized, (PlayerTarget.transform.parent.position - transform.position).magnitude - 1))
                {
                    r = false;
                    break;
                }
                else if (!Physics.Raycast(right.position, (PlayerTarget.transform.parent.position - transform.position).normalized, (PlayerTarget.transform.parent.position - transform.position).magnitude - 1))
                {
                    r = true;
                    break; // r is true by default
                }

                if (distance > 20)
                {
                    Debug.Log("escape");
                    break; // safety net escape (large geometry not dealt with yet anyway)
                }
            }

            // rotate camera away from obstruction
            if (!r)
            {
                distance *= -1;
            }

            transform.RotateAround(PlayerTarget.transform.position, controller.up, Time.deltaTime * distance * 0.3f);
        }
    }
}
