using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuff : MonoBehaviour
{
    // Start is called before the first frame update
    public int healVal;
    public float invTime;
    public PlayerCondition condition;

    public void Start()
    {
        condition = CharacterManager.Instance.player.condition;
    }

    public void Heal()
    {
        condition.Heal(healVal);
    }

    public void Invincibility()
    {

    }
}
