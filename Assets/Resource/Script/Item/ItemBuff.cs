using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ItemBuff : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController controller;
    public PlayerCondition condition;
    public float speedUpVal;
    public float maxTime;

    public void Start()
    {
      
    }


    public void Invincibility()
    {
        
        CharacterManager.Instance.player.condition.Invincibility();
    }

    public void SpeedUp()
    {
        CharacterManager.Instance.player.controller.SpeedUp();
    }
   
}
