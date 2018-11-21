using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject {
    [Header("Item")]
    public string itemName;
    public ItemTypeENUM itemType;
    public ItemRarityENUM itemRarity;
    public int quantity;
    public char buttonToPickUp;
    public GameObject itemModel;
    [Range(0, 1)]
    public float itemCondition;
}
