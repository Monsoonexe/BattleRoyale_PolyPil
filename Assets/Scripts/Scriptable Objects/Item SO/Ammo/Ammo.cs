using UnityEngine;

public class Ammo : Item
{
    [Header("Ammo")]
    public AmmoTypeENUM ammoType;
    public WeaponTypeENUM fitsWeaponType;

    private void Awake()
    {
        this.stackQuantity = 36;
        
    }

}
