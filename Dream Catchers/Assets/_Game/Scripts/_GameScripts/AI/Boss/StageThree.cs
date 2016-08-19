﻿using UnityEngine;
using System.Collections;

public class StageThree : Stages {

    public bool RightHand;
    int beginHealth;
    public int healthDifference;
    public int ball;
    public int swipe;
    public int smack;

    public int[] AttackPattern;
    int index;
    int rotation;


	// Use this for initialization
	void Start () {

        ball = 0;
        swipe = 0;
        smack = 0;
        rotation = 3;


	}

    public override void Play()
    {
        //one hit next stage
        if (Bc.RightHand == null && Bc.LeftHand == null)
        {
            Bc.currentStage = NextStage;
        }
        else if(Bc.RightHand == null)
        {
            if(Danger(Bc.LeftHand,false) == false)
            {
                chooseAttack(Bc.LeftHand,false);
            }

        }else if(Bc.LeftHand == null)
        {
            if (Danger(Bc.RightHand,false) == false)
            {
                chooseAttack(Bc.RightHand, false);
            }

        }else
        {
            bool r = Danger(Bc.RightHand, true);
            bool l;
            if(r == false)
            {
                l = Danger(Bc.LeftHand, true);
            }else
            {
                l = false;
            }

            if(r == false && l == false)
            {
                if (RightHand == true  && Bc.LeftHand.attack == HandScript.Mode.NONE)
                {
                    chooseAttack(Bc.RightHand, true);
                }
                else if(Bc.RightHand.attack == HandScript.Mode.NONE )
                {
                    chooseAttack(Bc.LeftHand, true);
                }
            }
            
        }


    }

    //choose an attack based on number
    public void chooseAttack(HandScript Hand, bool bothHands)
    {


        if(Hand.attack == HandScript.Mode.NONE)
        {
            if (bothHands == true)
            {
                RightHand = !RightHand;
            }

            if(rotation == 3)
            {
                int rand = Random.Range(0, 4);
                index = rand * 3;
                rotation = 0;
            }
            else
            {
                index++;
            }

            if(AttackPattern[index] == 0)
            {
                Hand.ThrowBall();
            }
            else if(AttackPattern[index] == 1)
            {
                Hand.Swipe();
            }
            else
            {
                Hand.ChargeSmackDown();
            }

            rotation++;
            //if(ball == 0)
            //{
            //    Hand.ThrowBall();
            //    ball++;

            //}else
            //{
            //    if(swipe >= ball + 3 && smack >= ball + 3)
            //    {
            //        Hand.ThrowBall();
            //        ball++;

            //    }else
            //    {
            //        float rand = Random.Range(0, 30);
            //        if (rand <= 10)
            //        {
                        
            //            Hand.ThrowBall();
            //            ball++;
            //        }
            //        else if (rand <= 20)
            //        {
                        
            //            Hand.Swipe();
            //            swipe++;
            //        }
            //        else
            //        {
                        
            //            Hand.ChargeSmackDown();
            //            smack++;
            //        }
            //    }
            //}
        }

    }

    public bool Danger(HandScript Hand, bool bothHands)
    {
        if(Bc.inDanger == false && Hand.attack == HandScript.Mode.BLOCK)
        {
            Hand.BlockReturn();
            return true;

        }else if(Bc.inDanger == true && ((bothHands == false && (Hand.attack == HandScript.Mode.NONE || Hand.attack == HandScript.Mode.THROW)) || (bothHands == true && Hand.attack == HandScript.Mode.NONE)))
        {
            Hand.Block();
            return true;

        }else if(Hand.attack == HandScript.Mode.BLOCK || Hand.attack == HandScript.Mode.UNBLOCK)
        {
            return true;
        }

        return false;
    }
}
