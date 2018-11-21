using UnityEngine;

public class Ammo : Item
{
    [Header("Ammo")]
    public AmmoTypeENUM ammoType;
    public WeaponTypeENUM fitsWeaponType;

    private void Awake()
    {
        this.itemType = ItemTypeENUM.ammo;
        this.stackQuantity = 36;
        
    }

}
