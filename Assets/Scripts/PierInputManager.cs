﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PierInputManager : MonoBehaviour
{
    public enum PlayerNumber { P1, P2 }
    public enum ButtonName { Move_Horizontal, Move_Vertical, Look_Horizontal, Look_Vertical, Left_Punch, Left_Kick, Right_Punch, Right_Kick, Run }
    public PlayerNumber playerNumber;
    public Player player;
    public void Setup()
    {
        if (player == null)
        {
            player = ReInput.players.GetPlayer((int)playerNumber);

        }
    }
    public void Awake()
    {
        Setup();
    }
    public float GetAxis( string axisName)
    {

        return player.GetAxis(axisName);


    }
    public float GetAxis(ButtonName AxisName)
    {

        return player.GetAxis(AxisName.ToString());


    }

    public bool GetButton( ButtonName buttonName)
    {
      
            return player.GetButton(buttonName.ToString());

    }

    public bool GetButtonDown( ButtonName buttonName)
    {

            return player.GetButtonDown(buttonName.ToString());

  
    }
   
    public  bool GetButtonUp(ButtonName buttonName)
    {
    
            return player.GetButtonUp(buttonName.ToString());

        
    }
}
