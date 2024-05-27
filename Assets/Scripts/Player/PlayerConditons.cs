using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConditions : MonoBehaviour
{
    public UIConditions uiCondition;

    Conditions health { get { return uiCondition.health; } }
    Conditions stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay;

    public event Action onTakeDamage;

    void Update()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0)
        {
            return false;
        }

        stamina.Subtract(amount);

        return true;
    }
    public void UpdateRunningState(bool isRunning)
    {
        if (stamina.curValue <= 0)
        {
            isRunning = false;
        }
    }
}
