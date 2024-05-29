using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerConditions : MonoBehaviour
{
    public UIConditions uiCondition;
    private Rigidbody playerRigidbody;
    private bool isZeroGravity =false ;
    private float zeroGravityEndTime = 0f;
    Conditions health { get { return uiCondition.health; } }
    Conditions stamina { get { return uiCondition.stamina; } }

   

    public event Action onTakeDamage;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (isZeroGravity && Time.time >= zeroGravityEndTime)
        {
            EndZeroGravity();
        }
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
    public bool UpdateRunningState()
    {
        return stamina.curValue >= 10; // 스테미너가 10 이상이면 true 반환, 그렇지 않으면 false 반환
    }
    public void ApplyZeroGravity(float duration)
    {
        Debug.Log("플레이어 컨디션 문제없음");
        isZeroGravity = true;
        zeroGravityEndTime = Time.time + duration;
        playerRigidbody.useGravity = false;

        if (duration <= 0f)
        {
            EndZeroGravity();
        }
    }

    private void EndZeroGravity()
    {
        isZeroGravity = false;
        playerRigidbody.useGravity = true;
    }

}
