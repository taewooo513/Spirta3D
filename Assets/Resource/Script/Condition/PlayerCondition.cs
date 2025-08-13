using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}
public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;
    Condition health { get { return uiCondition.headlth; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }
    public float noHungerHealthDecay;
    public event Action onTakeDamage;

    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        UpdateStamina();
        if (hunger.curValue <= 0)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }
        if (health.curValue <= 0)
        {
            Die();
        }
    }

    public void Heal(float amout)
    {
        health.Add(amout);
    }
    public void Eat(float amout)
    {
        hunger.Add(amout);
    }

    public void Die()
    {
    }

    public void UpdateStamina()
    {
        if (CharacterManager.Instance.player.controller.isRun)
        {
            stamina.Subtract(stamina.passiveValue * 2 * Time.deltaTime);
        }
        else
        {
            stamina.Add(stamina.passiveValue * Time.deltaTime);
        }

        if (stamina.curValue == 0)
        {
            CharacterManager.Instance.player.controller.isRun = false;
        }
    }
    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0f)
        {
            return false;
        }

        stamina.Subtract(amount);
        return true;
    }
}
