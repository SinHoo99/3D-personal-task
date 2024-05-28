using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Platform", menuName = "New Platform")]
public class PlatFormData : ScriptableObject
{
    
    [Header("Info")]
    public string displayName;
    public string description;
    public GameObject Prefab;
}
