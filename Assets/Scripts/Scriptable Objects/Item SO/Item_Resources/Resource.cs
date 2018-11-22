using UnityEngine;

public class Resource : Item {
    [Header("Resource")]
    public ResourceTypeENUM resourceType;

    private void Awake()
    {
        this.itemType = ItemTypeENUM.resource;
        this.itemName = "Resource: ";
        this.stackQuantity = 100;
    }
}
