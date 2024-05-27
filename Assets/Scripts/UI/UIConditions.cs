using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConditions : MonoBehaviour
{
    public Conditions health;
    public Conditions stamina;
    void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
