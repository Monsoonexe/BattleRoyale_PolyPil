using UnityEngine;

public class Weapon : Item {
    [Header("Weapon")]
    public AmmoTypeENUM ammoType;
    public WeaponTypeENUM weaponType;
    [Range(1, 2)]
    public int hands;
    public GameObject[] attachments;

    
    
}
