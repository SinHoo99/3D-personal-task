using UnityEngine;

public enum ItemType
{
    Resource,
    Equipable,
    Consumable
}

public enum ConsumableType
{
    Hunger,
    Health
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public int itemID;
    public string displayName;
    public string description;
    public ItemType type;
    public GameObject Prefab;

    //[Header("Stat")]
    //public int additionalStats;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    // [Header("Equip")]
    //public GameObject equipPrefab;
}