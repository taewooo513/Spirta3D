using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public ItemData itemData;
    public Equipment equip;
    public Action addItem;
    public Transform dropPosition;
    public void Awake()
    {
        CharacterManager.Instance.player = this;
        condition = GetComponent<PlayerCondition>();
        controller = GetComponent<PlayerController>();
        equip = GetComponent<Equipment>();
    }
}
