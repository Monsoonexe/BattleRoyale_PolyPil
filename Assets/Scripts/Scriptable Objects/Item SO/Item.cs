using UnityEngine;

public class Item : ScriptableObject {
    [Header("Item")]
    public string itemName;
    public ItemTypeENUM itemType;
    public ItemRarityENUM itemRarity;
    public int quantity;
    public int stackQuantity; // <= 1 means cannot stack
    public char buttonToPickUp = 'F';
    public Sprite itemIcon;
    public GameObject itemModel;
    [Range(0, 1)]
    public float itemCondition = 1.0f;
}
