using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {
    
    [Header("Weapon")]
    public WeaponTypeENUM weaponType;
    [Range(1, 2)]
    public int hands;

    [Header("Weapon Stats")]
    public int damage;
    public int bulletsPerMag;
    public float reloadSpeed;
    public float shotsPerSecond;
    //public AudioClip shotSound;


    [Header("Attachments")]
    public GameObject attach_barrel;
    public GameObject attach_barrelTip;
    public GameObject attach_mag;
    public GameObject attach_grip;
    public GameObject attach_stock;

    private List<Attachment> attachments = new List<Attachment>();

    private void Awake()
    {
        this.stackQuantity = 1;
    }

    private void GrabAttachementReferences()
    {
        //TODO Integrate attachments
    //    attachments.Add
    //    attach_barrel
    //    attach_barrel;
    //    attach_barrelTip;
    //    attach_mag;
    //    attach_grip;
    //    attach_stock;
    }



}
